using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using MessageBroker;
using Newtonsoft.Json;
using OrderService;

namespace ConsoleHelper.Demos
{
    public class OrderServiceDemo : IExchange
    {
        private readonly string _exchangeName = "OrderServiceDemo";
        private readonly string _orderServiceQueueName = "OrderServiceQueue";
        private readonly string _inventoryServiceQueueName = "InventoryServiceQueue";
        private readonly string _billingServiceQueueName = "BillingServiceQueue";
        private readonly string _shippingServiceQueueName = "ShippingServiceQueue";
        private Guid OrderId = Guid.NewGuid();


        public void Start(BrokerAppType app)
        {
            switch (app)
            {
                case BrokerAppType.Publisher:
                    StartRabbitMqPublisher();
                    break;
                case BrokerAppType.Consumer:
                    StartRabbitMqConsumer();
                    break;
            }
        }

        private void StartRabbitMqPublisher()
        {
            using (var repo = new OrderRepositoryMock())
            using (var publisher = new RabbitMqPublisherAdapter<Order>(_exchangeName, "topic", durable: false, autoDelete: true))
            using (var consumer = new RabbitMqConsumerAdapter<Product>(_exchangeName, "topic", $"{_orderServiceQueueName}.InventoryReserved", durable: false, autoDelete: true))
            {
                while (true)
                {
                    Console.WriteLine("Select option and press [enter]:");
                    Console.WriteLine("1. Submit Order");
                    Console.WriteLine("2. View Order");
                    var selection = Console.ReadLine();
                    Console.Clear();

                    switch (selection)
                    {
                        case "1":
                            var service = new OrderServiceCommand(repo, publisher, consumer);
                            var order = GenerateMockOrder();
                            service.Register();
                            service.Submit(order);
                            Console.WriteLine("Order submitted");
                            break;
                        case "2":
                            order = repo.Get(OrderId);
                            Console.WriteLine(JsonConvert.SerializeObject(order));
                            break;
                        default:
                            return;
                    }
                }
            }
        }

        private Order GenerateMockOrder()
        {
            var CustomerId = Guid.NewGuid();

            return new Order()
            {
                OrderId = OrderId,
                Customer = new OrderCustomer()
                {
                    OrderId = OrderId,
                    UserId = CustomerId,
                    FullName = "Brian Wied",
                    Email = "brian.wied@gmail.com"
                },
                Products = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        ProductId = ProductRepositoryMock.ProductOneId,
                        Quantity = 3,
                        IsReserved = false
                    }
                },
                Status = OrderStates.Pending
            };
        }

        private void StartRabbitMqConsumer()
        {
            Console.WriteLine("Selection option and press [enter]:");
            Console.WriteLine("1. OrderService");
            Console.WriteLine("2. InventoryService");
            Console.WriteLine("3. BillingService");
            Console.WriteLine("4. ShippingService");
            var selection = Console.ReadLine();
            Console.Clear();

            switch (selection)
            {
                case "1":
                    StartRabbitMqConsumer($"{_orderServiceQueueName}.log", "inventory.reserved");
                    break;
                case "2":
                    StartRabbitMqConsumer($"{_inventoryServiceQueueName}.log", "order.submitted");
                    var inventoryService = new InventoryService(new ProductRepositoryMock(),
                        new RabbitMqPublisherAdapter<Product>(_exchangeName, "topic", durable: false, autoDelete: true),
                        new RabbitMqConsumerAdapter<Order>(_exchangeName, "topic", $"{_inventoryServiceQueueName}.OrderSubmitted", durable: false, autoDelete: true));
                    inventoryService.Register();
                    break;
                case "3":
                    StartRabbitMqConsumer(_billingServiceQueueName, "order.submitted");
                    break;
                case "4":
                    StartRabbitMqConsumer(_shippingServiceQueueName, "order.submitted");
                    break;
                default:
                    return;
            }
        }

        private void StartRabbitMqConsumer(string selection, string routingKey = "")
        {
            try
            {
                using (var exchange = new RabbitMqConsumerAdapter<Order>(_exchangeName, "topic", selection, durable: false, autoDelete: true))
                {
                    exchange.Register(routingKey);
                    exchange.Received += (sender, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] {0}", message);
                    };
                    Console.WriteLine("Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}

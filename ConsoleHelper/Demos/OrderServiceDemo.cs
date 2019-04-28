using System;
using System.Text;
using Domain;
using MessageBroker;
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
            using (var repo = new RepositoryMock())
            using (var publisher = new RabbitMqPublisherAdapter<Order>(_exchangeName, "topic", durable: false, autoDelete: true))
            {
                while (true)
                {
                    Console.WriteLine("Select option and press [enter]:");
                    Console.WriteLine("1. Submit Order");
                    var selection = Console.ReadLine();
                    Console.Clear();

                    switch (selection)
                    {
                        case "1":
                            var service = new OrderServiceCommand(repo, publisher);
                            var order = new OrderServiceView(repo).Get(RepositoryMock.OrderId);
                            service.Submit(order);
                            Console.WriteLine("Order submitted");
                            break;
                        default:
                            return;
                    }
                }
            }
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
                    StartRabbitMqConsumer(_orderServiceQueueName);
                    break;
                case "2":
                    StartRabbitMqConsumer(_inventoryServiceQueueName, "order.submitted");
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

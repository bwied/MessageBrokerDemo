using MessageBroker;
using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace OrderService
{
    public class OrderServiceCommand : IOrderServiceCommand
    {
        private readonly IBrokerPublisherAdapter<Order> _publisher;
        private readonly IBrokerConsumerAdapter<Product> _consumer;
        private readonly IRepository<Order> _repository;

        public OrderServiceCommand(IRepository<Order> repository, IBrokerPublisherAdapter<Order> publisher, IBrokerConsumerAdapter<Product> consumer)
        {
            _repository = repository;
            _publisher = publisher;
            _consumer = consumer;
        }

        public void Submit(Order order)
        {
            try
            {
                if (_repository.Get(order.OrderId) == null)
                {
                    _repository.Save(order);
                    _publisher.Publish(order, "order.submitted");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine((object) ex);
                throw;
            }
        }

        public void Register()
        {
            _consumer.Received += OnInventoryReserved;
            _consumer.Register("inventory.reserved");
        }

        private void UpdateOrderInventoryStatus(Order order)
        {
            try
            {
                if (order.Products.All(x => x.IsReserved))
                {
                    order.Status.InventoryState = "reserved";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine((object)ex);
                throw;
            }
        }

        private void OnInventoryReserved(object sender, DeliveryEventArgs e)
        {
            var order = _repository.Get(new Guid(e.BasicProperties.CorrelationId));
            var product = JsonConvert.DeserializeObject<Product>(Encoding.UTF8.GetString(e.Body));

            try
            {
                var orderProduct = order.Products.SingleOrDefault(x => x.ProductId == product.ProductId);

                if (orderProduct != null)
                {
                    orderProduct.IsReserved = true;
                    _repository.Save(order);
                    UpdateOrderInventoryStatus(order);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine((object)ex);
                throw;
            }
        }
    }

    public class InventoryService
    {
        private readonly IBrokerPublisherAdapter<Product> _publisher;
        private readonly IBrokerConsumerAdapter<Order> _consumer;
        private readonly IRepository<Product> _repository;

        public InventoryService(IRepository<Product> repository, IBrokerPublisherAdapter<Product> publisher, IBrokerConsumerAdapter<Order> consumer)
        {
            _repository = repository;
            _publisher = publisher;
            _consumer = consumer;
        }

        public void ReserveInventory(Guid productId, Decimal quantity, Guid orderId)
        {
            try
            {
                var product = _repository.Get(productId);

                if (product.QuantityOnHand >= quantity)
                {
                    product.QuantityOnHand -= quantity;
                    product.QuantityReserved += quantity;
                    _repository.Save(product);
                    Console.WriteLine("Inventory Reserved");
                    _publisher.Publish(product, "inventory.reserved", orderId.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine((object)ex);
                throw;
            }
        }

        public void Register()
        {
            _consumer.Received += OnOrderSubmitted;
            _consumer.Register("order.submitted");
        }

        private void OnOrderSubmitted(object sender, DeliveryEventArgs e)
        {
            var order = JsonConvert.DeserializeObject<Order>(Encoding.UTF8.GetString(e.Body));

            foreach (var orderProduct in order.Products)
            {
                ReserveInventory(orderProduct.ProductId, orderProduct.Quantity, order.OrderId);
            }
        }
    }

    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal QuantityOnOrder { get; set; }
        public decimal QuantityReserved { get; set; }
    }
}
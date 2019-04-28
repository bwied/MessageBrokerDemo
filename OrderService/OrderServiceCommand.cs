using MessageBroker;
using System;

namespace OrderService
{
    public class OrderServiceCommand : IOrderServiceCommand
    {
        private readonly IBrokerPublisherAdapter<Order> _publisher;
        private readonly IRepository<Order> _repository;

        public OrderServiceCommand(IRepository<Order> repository, IBrokerPublisherAdapter<Order> publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public void Submit(Order order)
        {
            try
            {
                if (order.Status == OrderStates.Submitted)
                    return;
                order.Status = OrderStates.Submitted;
                _repository.Save(order);
                _publisher.Publish(order, "order.submitted");
            }
            catch (Exception ex)
            {
                Console.WriteLine((object) ex);
                throw;
            }
        }
    }

    //public class InventoryService
    //{
    //    private readonly IBrokerPublisherAdapter<Product> _publisher;
    //    private readonly IRepository<Product> _repository;

    //    public InventoryService(IRepository<Product> repository, IBrokerPublisherAdapter<Product> publisher)
    //    {
    //        _repository = repository;
    //        _publisher = publisher;
    //    }

    //    public void ReserveInventory(Guid productId, Decimal quantity)
    //    {
    //        try
    //        {
    //            if (order.Status == OrderStates.Submitted)
    //                return;
    //            order.Status = OrderStates.Submitted;
    //            _repository.Save(order);
    //            _publisher.Publish(order, "order.submitted");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine((object)ex);
    //            throw;
    //        }
    //    }
    //}

    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal QuantityOnOrder { get; set; }
        public decimal QuantityReserved { get; set; }
    }
}
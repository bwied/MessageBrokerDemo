using System;

namespace OrderService
{
    public class OrderServiceView : IOrderServiceView
    {
        private readonly IRepository<Order> _repository;

        public OrderServiceView(IRepository<Order> repository)
        {
            this._repository = repository;
        }

        public Order Get(Guid orderId)
        {
            return this._repository.Get(orderId);
        }
    }
}

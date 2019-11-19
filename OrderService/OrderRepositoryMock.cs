using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService
{
    public class OrderRepositoryMock : IRepository<Order>
    {
        public static readonly Guid OrderId = Guid.NewGuid();

        public Order Get(Guid id)
        {
            return DataStoreMock.Orders.SingleOrDefault(x => x.OrderId == id);
        }

        public void Save(Order order)
        {
            var index = DataStoreMock.Orders.IndexOf(order);

            if (index == -1)
            {
                DataStoreMock.Orders.Add(order);
            }
            else
            {
                DataStoreMock.Orders[index] = order;
            }
        }

        private static class DataStoreMock
        {
            private static readonly Guid CustomerId = Guid.NewGuid();

            public static readonly List<Order> Orders = new List<Order>();
        }

        public void Dispose()
        {
            
        }
    }
}

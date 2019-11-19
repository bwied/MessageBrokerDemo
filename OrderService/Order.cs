using System;
using System.Collections.Generic;

namespace OrderService
{
  [Serializable]
  public class Order
  {
    public Guid OrderId { get; set; }

    public OrderCustomer Customer { get; set; }

    public List<OrderProduct> Products { get; set; }

    public OrderState Status { get; set; }
  }

  [Serializable]
  public class OrderProduct
  {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public bool IsReserved { get; set; }
  }
}

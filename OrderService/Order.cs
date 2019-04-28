using System;
using System.Collections.Generic;

namespace OrderService
{
  [Serializable]
  public class Order
  {
    public Guid OrderId { get; set; }

    public OrderCustomer Customer { get; set; }

    public Dictionary<Guid, int> Products { get; set; }

    public OrderState Status { get; set; }
  }
}

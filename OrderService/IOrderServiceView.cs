using System;

namespace OrderService
{
  public interface IOrderServiceView
  {
    Order Get(Guid orderId);
  }
}

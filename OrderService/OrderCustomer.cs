using System;

namespace OrderService
{
  [Serializable]
  public class OrderCustomer
  {
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }
  }
}

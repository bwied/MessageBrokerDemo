using System;

namespace OrderService
{
    [Serializable]
    public class OrderState : IComparable
    {
        public string InventoryState { get; set; }

        public string PaymentState { get; set; }

        public string ShippingState { get; set; }

        public int CompareTo(object obj)
        {
            switch (obj)
            {
                case OrderState orderState:
                    return string.Compare(this.InventoryState, orderState.InventoryState, StringComparison.Ordinal) + 
                           string.Compare(this.PaymentState, orderState.PaymentState, StringComparison.Ordinal) + 
                           string.Compare(this.ShippingState, orderState.ShippingState, StringComparison.Ordinal);
                case null:
                    return 1;
                default:
                    throw new ArgumentException("Object is not an OrderState");
            }
        }
    }
}

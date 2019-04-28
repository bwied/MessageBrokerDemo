namespace OrderService
{
    public static class OrderStates
    {
        public static OrderState Pending
        {
            get
            {
                return new OrderState()
                {
                    InventoryState = "pending"
                };
            }
        }

        public static OrderState Submitted => new OrderState() { InventoryState = "reserved", PaymentState = "info-received", ShippingState = "address-received" };

        public static OrderState Processed => new OrderState() {InventoryState = "pick-slip-generated", PaymentState = "processed", ShippingState = "address-validated"};

        public static OrderState Picked => new OrderState() {InventoryState = "picked", PaymentState = "processed", ShippingState = "address-validated"};

        public static OrderState Packed => new OrderState() {InventoryState = "packed", PaymentState = "processed", ShippingState = "address-validated"};

        public static OrderState Shipped => new OrderState() {InventoryState = "shipped", PaymentState = "processed", ShippingState = "address-validated"};

        public static OrderState Delivered => new OrderState() {InventoryState = "delivered", PaymentState = "processed", ShippingState = "address-validated"};
    }
}

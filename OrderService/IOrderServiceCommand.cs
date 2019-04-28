namespace OrderService
{
    public interface IOrderServiceCommand
    {
        void Submit(Order order);
    }
}
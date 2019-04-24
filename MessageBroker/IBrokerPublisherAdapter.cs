namespace MessageBroker
{
    public interface IBrokerPublisherAdapter<in T>
    {
        void Publish(T obj, string routingKey);
    }
}
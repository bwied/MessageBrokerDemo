using System;

namespace MessageBroker
{
    public interface IBrokerPublisherAdapter<in T> : IDisposable
    {
        void Publish(T obj, string routingKey);
    }
}
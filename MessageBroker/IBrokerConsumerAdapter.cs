using System;

namespace MessageBroker
{
    public interface IBrokerConsumerAdapter<in T> : IDisposable
    {
        event EventHandler<DeliveryEventArgs> Received;
        void Register(string routingKey = "");
    }
}
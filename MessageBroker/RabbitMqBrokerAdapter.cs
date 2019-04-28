using System;
using RabbitMQ.Client;

namespace MessageBroker
{
    public class RabbitMqBrokerAdapter : IDisposable
    {
        protected readonly IModel Channel;
        protected readonly string ExchangeName;

        protected RabbitMqBrokerAdapter(string exchangeName, string exchangeType, string hostName = "localhost", bool durable = false, bool autoDelete = false)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            var connection = factory.CreateConnection();
            Channel = connection.CreateModel();
            ExchangeName = exchangeName;
            Channel.ExchangeDeclare(ExchangeName, exchangeType, durable, autoDelete);
        }

        public void Dispose()
        {
            Channel.Dispose();
        }
    }
}

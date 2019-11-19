using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace MessageBroker
{
    public class RabbitMqPublisherAdapter<T> : RabbitMqBrokerAdapter, IBrokerPublisherAdapter<T>
    {
        public RabbitMqPublisherAdapter(string exchangeName, string exchangeType, string hostName = "localhost", bool durable = false, bool autoDelete = false)
            : base(exchangeName, exchangeType, hostName, durable, autoDelete) { }

        public void Publish(T obj, string routingKey, string correlationId = "")
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Channel.BasicPublish(ExchangeName, routingKey, new BasicProperties() { CorrelationId = correlationId}, Encoding.UTF8.GetBytes(json));
        }
    }
}

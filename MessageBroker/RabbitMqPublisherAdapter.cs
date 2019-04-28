using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MessageBroker
{
    public class RabbitMqPublisherAdapter<T> : RabbitMqBrokerAdapter, IBrokerPublisherAdapter<T>
    {
        public RabbitMqPublisherAdapter(string exchangeName, string exchangeType, string hostName = "localhost", bool durable = false, bool autoDelete = false)
            : base(exchangeName, exchangeType, hostName, durable, autoDelete) { }

        public void Publish(T obj, string routingKey)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Channel.BasicPublish(ExchangeName, routingKey, null, Encoding.UTF8.GetBytes(json));
        }
    }
}

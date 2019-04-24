using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MessageBroker
{
    public class RabitMqPublisherAdapter<T> : IBrokerPublisherAdapter<T>
    {
        private readonly RabbitMqProxy _exchange = new RabbitMqProxy();
        private readonly string _exchangeName;
        private readonly string _exchangeType;
        private readonly bool _durable;
        private readonly bool _autoDelete;
        private IModel _channel;

        public RabitMqPublisherAdapter(string exchangeName, string exchangeType, bool durable = false, bool autoDelete = false)
        {
            _exchangeName = exchangeName;
            _exchangeType = exchangeType;
            _durable = durable;
            _autoDelete = autoDelete;
        }

        public void Publish(T obj, string routingKey)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Channel.BasicPublish(_exchangeName, routingKey, null, Encoding.UTF8.GetBytes(json));
        }

        private IModel Channel => _channel ?? (_channel = _exchange.GetChannel(_exchangeName, _exchangeType, _durable, _autoDelete));
    }
}

using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageBroker
{
    public class RabbitMqConsumerAdapter<T> : RabbitMqBrokerAdapter, IBrokerConsumerAdapter<T>
    {
        public event EventHandler<DeliveryEventArgs> Received;
        private readonly EventingBasicConsumer _consumer;
        private readonly string _queueName;
        private readonly bool _durable;
        private readonly bool _exclusive;
        private readonly bool _autoDelete;

        public RabbitMqConsumerAdapter(string exchangeName, string exchangeType, string queueName, string hostName = "localhost", bool durable = false, bool exclusive = false, bool autoDelete = false)
            : base(exchangeName, exchangeType, hostName, durable, autoDelete)
        {
            _queueName = queueName;
            _consumer = new EventingBasicConsumer(Channel);
            _consumer.Received += OnReceived;
            _durable = durable;
            _exclusive = exclusive;
            _autoDelete = autoDelete;
            base.Channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);
        }

        public void Register(string routingKey = "")
        {
            base.Channel.QueueDeclare(_queueName, _durable, _exclusive, _autoDelete);
            base.Channel.QueueBind(_queueName, ExchangeName, routingKey);
            base.Channel.BasicConsume(_queueName, true, _consumer);
        }

        private void OnReceived(object sender, BasicDeliverEventArgs ea)
        {
            if (Received == null) return;

            var eventAdapter = new DeliveryEventArgs()
            {
                Exchange = ea.Exchange,
                BasicProperties = ea.BasicProperties,
                Body = ea.Body
            };

            Received.Invoke(this, eventAdapter);
        }
    }
}

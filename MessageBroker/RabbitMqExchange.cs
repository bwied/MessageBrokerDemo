using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageBroker
{
    public class RabbitMqExchange : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqExchange()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }

        public Action<string, string, IBasicProperties, byte[]> GetPublisher(string exchange, string exchangeType, bool durable = false, bool autoDelete = false)
        {
            _channel.ExchangeDeclare(exchange, exchangeType, durable, autoDelete);

            return _channel.BasicPublish;
        }

        public void RegisterDynamicConsumer(string exchange, string exchangeType, string routingKey, bool durable = false, bool autoDelete = false)
        {
            _channel.ExchangeDeclare(exchange, exchangeType, durable, autoDelete);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, exchange, routingKey);

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };
            _channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public void RegisterConsumer(string exchangeName, string exchangeType, string queueName, string routingKey, bool durable = false, bool autoDelete = false, bool exclusive = true)
        {
            _channel.ExchangeDeclare(exchangeName, exchangeType, durable, autoDelete);
            _channel.QueueDeclare(queueName, durable, exclusive, autoDelete);
            _channel.QueueBind(queueName, exchangeName, routingKey);

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };
            _channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}

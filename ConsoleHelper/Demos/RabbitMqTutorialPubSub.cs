using System;
using System.Text;
using System.Threading;
using Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsoleHelper.Demos
{
    public class RabbitMqTutorialPubSub : IExchange
    {
        private readonly string[] _args = new[]
            {"First message.", "Second message..", "Third message...", "Fourth message....", "Fifth message....."};
        private int counter = 0;

        public void Start(BrokerAppType app)
        {
            switch (app)
            {
                case BrokerAppType.Publisher:
                    for (int i = 0; i < _args.Length; i++)
                    {
                        StartRabbitMqPublisher();
                        counter++;
                    }

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                    break;
                case BrokerAppType.Consumer:
                    StartRabbitMqConsumer();
                    break;
            }
        }

        private void StartRabbitMqPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout", autoDelete: true);

                var message = _args[counter];
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                    routingKey: "",
                    basicProperties: null,
                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        private void StartRabbitMqConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout", autoDelete: true);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                    exchange: "logs",
                    routingKey: "");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}

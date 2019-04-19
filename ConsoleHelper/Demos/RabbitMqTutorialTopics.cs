using System;
using System.Linq;
using System.Text;
using Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsoleHelper.Demos
{
    public class RabbitMqTutorialTopics
    {
        public void Start(BrokerAppType app)
        {
            switch (app)
            {
                case BrokerAppType.Publisher:
                    StartRabbitMqPublisher();
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
                channel.ExchangeDeclare(exchange: "topic_logs",type: "topic", autoDelete: true);
                while (true)
                {
                    Console.WriteLine(
                        "Enter one or more routing keys and one message using commas between parameters(kern.*, *.critical, This is the error message):");
                    var args = Console.ReadLine()?.Split(',') ?? new string[0];
                    var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
                    var message = (args.Length > 1)
                        ? string.Join(" ", args.Skip(1).ToArray())
                        : "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "topic_logs",
                        routingKey: routingKey,
                        basicProperties: null,
                        body: body);
                    Console.Clear();
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
                }
            }
        }

        private void StartRabbitMqConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs", type: "topic", autoDelete: true);
                var queueName = channel.QueueDeclare().QueueName;
                Console.WriteLine("Enter one or more binding keys using spaces between parameters(*.topic.*, topic.*.topic, *.topic, #):");
                var args = Console.ReadLine()?.Split(' ') ?? new string[0];

                if (args.Length < 1)
                {
                    Console.Error.WriteLine("Usage: {0} [binding_key...]",
                        Environment.GetCommandLineArgs()[0]);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                    Environment.ExitCode = 1;
                    return;
                }

                foreach (var bindingKey in args)
                {
                    channel.QueueBind(queue: queueName,
                        exchange: "topic_logs",
                        routingKey: bindingKey);
                }

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                        routingKey,
                        message);
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

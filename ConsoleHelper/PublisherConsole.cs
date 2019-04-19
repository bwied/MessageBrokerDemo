using System;
using System.Text;
using RabbitMQ.Client;

namespace ConsoleHelper
{
    public class PublisherConsole
    {
        private readonly string _exchange;
        private readonly string _routingKey;
        private readonly IBasicProperties _basicProperties;

        public PublisherConsole(string exchange, string routingKey, IBasicProperties basicProperties)
        {
            _exchange = exchange;
            _routingKey = routingKey;
            _basicProperties = basicProperties;
        }

        public void StartPublisher(Action<string, string, IBasicProperties, byte[]> basicPublish)
        {
            var statusMessage = string.Empty;
            while (true)
            {
                Console.WriteLine("Type message and press [enter].  Type 'exit' to end program. {0}", statusMessage);
                var message = Console.ReadLine();
                Console.Clear();

                if (message != "exit" && !string.IsNullOrEmpty(message))
                {
                    var body = Encoding.UTF8.GetBytes(message);

                    basicPublish(_exchange, _routingKey, _basicProperties, body);
                    statusMessage = $" [x] Sent {message}";
                }
                else
                {
                    break;
                }

                
            }

        }
    }
}

using Domain;
using MessageBroker;
using RabbitMQ.Client;

namespace ConsoleHelper.Demos
{
    public class DirectExchange : IExchange
    {
        private readonly string _exchangeName = "DirectExchangeDemo";

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
            using (var exchange = new RabbitMqProxy())
            {
                var action = exchange.GetPublisher(_exchangeName, ExchangeType.Direct, false, true);
                var console = new PublisherConsole(_exchangeName, "", null);
                console.StartPublisher(action);
            }
        }

        private void StartRabbitMqConsumer()
        {
            using (var exchange = new RabbitMqProxy())
            {
                exchange.RegisterConsumer(_exchangeName, ExchangeType.Direct, "DirectQueue", "", false, true, false);
            }
        }
    }
}

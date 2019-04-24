using Domain;
using MessageBroker;
using RabbitMQ.Client;

namespace ConsoleHelper.Demos
{
    public class RoutingKeyFanoutExchange : IExchange
    {
        private readonly string _exchangeName = "RoutingKeyFanoutExchangeDemo";

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
                var action = exchange.GetPublisher(_exchangeName, ExchangeType.Fanout, false, true);
                var console = new PublisherConsole(_exchangeName, "DummyRoute", null);
                console.StartPublisher(action);
            }
        }

        private void StartRabbitMqConsumer()
        {
            using (var exchange = new RabbitMqProxy())
            {
                exchange.RegisterDynamicConsumer(_exchangeName, ExchangeType.Fanout, "DummyRoute", false, true);
            }
        }
    }
}

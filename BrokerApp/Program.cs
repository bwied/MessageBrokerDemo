using ConsoleHelper;
using System;
using ConsoleHelper.Demos;
using Domain;
using RabbitMQ.Client;

namespace BrokerApp
{
    class Program
    {
        static void Main(string[] args)
        {

            if (HasValidArguments(args, out object app, out object subApp))
            {
                switch ((DemoAppLauncherSelection)app)
                {
                    case DemoAppLauncherSelection.TutorialHelloWorld:
                        new RabbitMqTutorial1().Start((BrokerAppType)subApp);
                        break;
                    case DemoAppLauncherSelection.TutorialWorkQueue:
                        new RabbitMqTutorialWorkQueues().Start((BrokerAppType)subApp);
                        break;
                    case DemoAppLauncherSelection.FanoutExchange:
                        new FanoutExchange("FanoutExchangeDemo").Start((BrokerAppType)subApp);
                        break;
                    case DemoAppLauncherSelection.DirectExchange:
                        new DirectExchange("DirectExchangeDemo").Start((BrokerAppType)subApp);
                        break;
                    case DemoAppLauncherSelection.RoutingKeyFanoutExchange:
                        new RoutingKeyFanoutExchange("RoutingKeyFanoutExchangeDemo").Start((BrokerAppType)subApp);
                        break;
                    case DemoAppLauncherSelection.TutorialPubSub:
                        new RabbitMqTutorialPubSub().Start((BrokerAppType)subApp);
                        break;
                    case DemoAppLauncherSelection.TutorialRouting:
                        new RabbitMqTutorialRouting().Start((BrokerAppType)subApp);
                        break;
                }
            }
        }

        private static bool HasValidArguments(string[] args, out object app, out object subApp)
        {
            app = subApp = String.Empty;

            return args != null &&
                   args.Length > 1 &&
                   Enum.TryParse(typeof(DemoAppLauncherSelection), args[0], false, out app) &&
                   Enum.TryParse(typeof(BrokerAppType), args[1], false, out subApp);
        }
    }
}

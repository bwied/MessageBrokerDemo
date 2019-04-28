using System.Collections.Generic;
using ConsoleHelper.Demos;
using Domain;

namespace BrokerApp
{
    public static class ApplicationDictionary
    {
        public static Dictionary<DemoAppLauncherSelection, IExchange> GetApplicationDictionary()
        {
            return new Dictionary<DemoAppLauncherSelection, IExchange>()
            {
                {DemoAppLauncherSelection.TutorialHelloWorld, new RabbitMqTutorial1()},
                {DemoAppLauncherSelection.TutorialWorkQueue, new RabbitMqTutorialWorkQueues()},
                {DemoAppLauncherSelection.TutorialPubSub, new RabbitMqTutorialPubSub()},
                {DemoAppLauncherSelection.TutorialRouting, new RabbitMqTutorialRouting()},
                {DemoAppLauncherSelection.TutorialTopics, new RabbitMqTutorialTopics()},
                {DemoAppLauncherSelection.DirectExchange, new DirectExchange()},
                {DemoAppLauncherSelection.FanoutExchange, new FanoutExchange()},
                {DemoAppLauncherSelection.RoutingKeyFanoutExchange, new RoutingKeyFanoutExchange()},
                {DemoAppLauncherSelection.OrderServiceDemo, new OrderServiceDemo() }
            };
        }
    }
}
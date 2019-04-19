using ConsoleHelper;
using System;
using System.Collections.Generic;
using ConsoleHelper.Demos;
using Domain;
using RabbitMQ.Client;
using static System.String;

namespace BrokerApp
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            if (!HasValidArguments(args, out var app, out var subApp)) return;

            LaunchApplication(app, subApp);
        }

        private static void LaunchApplication(object app, object subApp)
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
                    new FanoutExchange().Start((BrokerAppType)subApp);
                    break;
                case DemoAppLauncherSelection.DirectExchange:
                    new DirectExchange().Start((BrokerAppType)subApp);
                    break;
                case DemoAppLauncherSelection.RoutingKeyFanoutExchange:
                    new RoutingKeyFanoutExchange().Start((BrokerAppType)subApp);
                    break;
                case DemoAppLauncherSelection.TutorialPubSub:
                    new RabbitMqTutorialPubSub().Start((BrokerAppType)subApp);
                    break;
                case DemoAppLauncherSelection.TutorialRouting:
                    new RabbitMqTutorialRouting().Start((BrokerAppType)subApp);
                    break;
                case DemoAppLauncherSelection.TutorialTopics:
                    new RabbitMqTutorialTopics().Start((BrokerAppType)subApp);
                    break;
                case DemoAppLauncherSelection.TutorialRpc:
                case DemoAppLauncherSelection.Exit:
                default:
                    Console.WriteLine("Selection not implemented. Press [enter] to return to main menu.");
                    Console.ReadLine();
                    break;
            }
        }

        private static bool HasValidArguments(IReadOnlyList<string> args, out object app, out object subApp)
        {
            app = subApp = Empty;

            return args != null &&
                   args.Count > 1 &&
                   Enum.TryParse(typeof(DemoAppLauncherSelection), args[0], false, out app) &&
                   Enum.TryParse(typeof(BrokerAppType), args[1], false, out subApp);
        }
    }
}

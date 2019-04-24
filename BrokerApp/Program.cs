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
        private static Dictionary<DemoAppLauncherSelection, IExchange> _applicationDictionary;

        private static void Main(string[] args)
        {
            if (!HasValidArguments(args, out var app, out var subApp)) return;

            _applicationDictionary = ApplicationDictionary.GetApplicationDictionary();
            LaunchApplication(app, subApp);
        }

        private static void LaunchApplication(object app, object subApp)
        {
            if (_applicationDictionary.ContainsKey((DemoAppLauncherSelection)app))
            {
                _applicationDictionary[(DemoAppLauncherSelection)app].Start((BrokerAppType)subApp);
            }
            else
            {
                Console.WriteLine("Selection not implemented. Press [enter] to return to main menu.");
                Console.ReadLine();
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

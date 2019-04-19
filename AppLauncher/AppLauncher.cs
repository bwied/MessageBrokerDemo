using Domain;
using System;
using System.Collections.Generic;
using ConsoleHelper;

namespace AppLauncher
{
    public static class AppLauncher
    {
        private const string WorkingDirectory = @"c:\Users\Brian.Wied\source\repos\MessageBrokerDemo\BrokerApp\";

        public static void Main()
        {
            var windowManager = new WindowLayoutManager();

            while (true)
            {
                Console.WriteLine("Type the number of the application to start and then press [enter].");

                foreach (var value in Enum.GetValues(typeof(DemoAppLauncherSelection)))
                {
                    Console.WriteLine($"{(int)value} - {value}");
                }

                var selection = Console.ReadLine();
                Console.Clear();

                if (string.IsNullOrEmpty(selection) || !Enum.IsDefined(typeof(DemoAppLauncherSelection), Convert.ToInt32(selection)))
                    continue;

                var appSelection = Enum.Parse<DemoAppLauncherSelection>(selection);

                if (appSelection != DemoAppLauncherSelection.Exit)
                {
                    windowManager.DisplayBrokerConsole(WorkingDirectory, appSelection);
                }
                else
                {
                    windowManager.CloseWindows();
                    break;
                }
            }
        }
    }
}

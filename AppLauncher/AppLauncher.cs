using Domain;
using System;
using System.Collections.Generic;
using ConsoleHelper;

namespace AppLauncher
{
    partial class AppLauncher
    {
        public static void Main()
        {
            var workingDirectory = @"c:\Users\Brian.Wied\source\repos\MessageBrokerDemo\BrokerApp\";
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

                DemoAppLauncherSelection appSelection = Enum.Parse<DemoAppLauncherSelection>(selection);

                if (appSelection != DemoAppLauncherSelection.Exit)
                {
                    windowManager.DisplayBrokerConsole(workingDirectory, appSelection);
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

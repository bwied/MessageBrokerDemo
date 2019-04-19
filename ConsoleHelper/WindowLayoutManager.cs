using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace ConsoleHelper
{
    public class WindowLayoutManager
    {
        private List<IntPtr> _windowHandles = new List<IntPtr>();
        private List<WindowLayout> _windowLayouts = new List<WindowLayout>();
        private int xOffset = -7;
        private int xDefaultPosition = 0;
        private int yDefaultPosition = 0;

        public void DisplayBrokerConsole(string workingDirectory, DemoAppLauncherSelection appSelection)
        {
            while (true)
            {
                Console.WriteLine(Enum.GetName(typeof(DemoAppLauncherSelection), appSelection));
                Console.WriteLine("Type the number of the application to start and then press [enter].");

                foreach (var value in Enum.GetValues(typeof(BrokerAppLauncherSelection)))
                {
                    Console.WriteLine($"{(int)value} - {value}");
                }

                var selection = Console.ReadLine();
                Console.Clear();

                if (string.IsNullOrEmpty(selection))
                    continue;

                BrokerAppLauncherSelection brokerAppSelection = (BrokerAppLauncherSelection)Enum.Parse(typeof(BrokerAppLauncherSelection), selection);

                if (brokerAppSelection != BrokerAppLauncherSelection.Exit)
                {
                    switch (brokerAppSelection)
                    {
                        case BrokerAppLauncherSelection.Publisher:
                            DisplayPublisherWindow(workingDirectory, appSelection);
                            ProcessCommand.FocusProcess(_windowHandles.Last());
                            break;
                        case BrokerAppLauncherSelection.Consumer:
                            DisplayConsumerWindow(workingDirectory, appSelection);
                            break;
                        case BrokerAppLauncherSelection.All:
                            DisplayConsumerWindow(workingDirectory, appSelection);
                            DisplayConsumerWindow(workingDirectory, appSelection);
                            DisplayConsumerWindow(workingDirectory, appSelection);
                            DisplayPublisherWindow(workingDirectory, appSelection);
                            var publisherHandle = _windowHandles.Last();
                            ProcessCommand.FocusProcess(publisherHandle);
                            break;
                    }
                }
                else
                {
                    CloseWindows();
                    _windowLayouts = new List<WindowLayout>();
                    _windowHandles = new List<IntPtr>();
                    break;
                }
            }
        }

        private void DisplayPublisherWindow(string workingDirectory, DemoAppLauncherSelection appSelection)
        {
            var windowLayout = GetPublisherWindowLayout(appSelection);
            var windowHandle = ProcessCommand.StartProgram(workingDirectory, appSelection, BrokerAppType.Publisher, windowLayout);
            _windowHandles.Add(windowHandle);
        }

        private void DisplayConsumerWindow(string workingDirectory, DemoAppLauncherSelection appSelection)
        {
            var windowLayout = GetConsumerWindowLayout(appSelection);
            var windowHandle = ProcessCommand.StartProgram(workingDirectory, appSelection, BrokerAppType.Consumer, windowLayout);
            _windowHandles.Add(windowHandle);
        }

        private WindowLayout GetPublisherWindowLayout(DemoAppLauncherSelection appSelection)
        {
            var width = 700;
            var height = 500;
            var windowLayout = GetWindowLayout(appSelection, width, height);

            _windowLayouts.Add(windowLayout);

            return windowLayout;
        }

        private WindowLayout GetConsumerWindowLayout(DemoAppLauncherSelection appSelection)
        {
            var width = 350;
            var height = 500;
            var windowLayout = GetWindowLayout(appSelection, width, height);

            _windowLayouts.Add(windowLayout);

            return windowLayout;
        }

        private WindowLayout GetWindowLayout(DemoAppLauncherSelection appSelection, int width, int height)
        {
            var count = _windowLayouts.Count;

            if (count == 0)
            {
                return new WindowLayout() { Height = height, Width = width, StartX = xDefaultPosition + xOffset, StartY = yDefaultPosition };
            }
            else
            {
                var last = _windowLayouts.Last();

                return new WindowLayout() { Height = height, Width = width, StartX = last.StartX + last.Width + xOffset, StartY = yDefaultPosition };
            }
        }

        public void CloseWindows()
        {
            ProcessCommand.CloseWindows(_windowHandles);
        }
    }
}

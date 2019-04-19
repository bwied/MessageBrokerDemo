using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Domain;

namespace ConsoleHelper
{
    public class ProcessCommand
    {
        public static void FocusProcess(IntPtr handle)
        {
            SetForegroundWindow(handle);
        }

        public static void CloseWindows(List<IntPtr> handles)
        {
            foreach (var handle in handles)
            {
                CloseWindow(handle);
            }
        }

        public static IntPtr StartProgram(string workingDirectory, DemoAppLauncherSelection app, BrokerAppType subApp, WindowLayout wp)
        {
            var startInfo = new ProcessStartInfo("dotnet");
            startInfo.Arguments = $"run --no-build {app.ToString()} {subApp.ToString()}";
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = workingDirectory;

            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            Thread.Sleep(500);
            var handle = process.MainWindowHandle;
            if (!MoveWindow((IntPtr)handle, wp.StartX, wp.StartY, wp.Width, wp.Height, true))
            {
                throw new Win32Exception();
            }

            return (IntPtr) handle;
        }

        #region Private

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);

        [DllImport("user32.dll")]
        private static extern int GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private const UInt32 WM_CLOSE = 0x0010;

        private static void CloseWindow(IntPtr hwnd)
        {
            ProcessCommand.SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        #endregion
    }
}

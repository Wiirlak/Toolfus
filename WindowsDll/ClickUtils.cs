using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ToolfusDll
{
    public class ClickUtils
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        public static IntPtr LParams(int wLow, int wHigh)
        {
            return (IntPtr) ( (short) wHigh << 16 | wLow & ushort.MaxValue);
        }

        public void ClickBackgroundWindowPosition(int pid, int x, int y, char lr)
        {
            IntPtr hWnd = Process.GetProcessById(pid).MainWindowHandle;
            POINT pt = new POINT { X = x, Y = y };
            ScreenToClient(hWnd, ref pt);
            Console.WriteLine($"[ClickUtils] ClickBackgroundWindowPosition pid={pid} screen=({x},{y}) client=({pt.X},{pt.Y}) button={lr}");
            uint downMsg = lr == 'r' ? 516U : 513U;
            uint upMsg   = lr == 'r' ? 517U : 514U;
            SendMessage(hWnd, downMsg, IntPtr.Zero, LParams(pt.X, pt.Y));
            SendMessage(hWnd, upMsg,   IntPtr.Zero, LParams(pt.X, pt.Y));
        }

        public void ClickBackgroundWindow(int pid, char lr)
        {
            IntPtr hWnd = Process.GetProcessById(pid).MainWindowHandle;
            Console.WriteLine($"[ClickUtils] ClickBackgroundWindow pid={pid} button={lr}");
            // 513U = WM_LBUTTONDOWN, 516U = WM_RBUTTONDOWN
            SendMessage(hWnd, (lr == 'r' ? 516U : 513U), IntPtr.Zero, LParams(0, 0));
            SendMessage(hWnd, (lr == 'r' ? 517U : 514U), IntPtr.Zero, LParams(0, 0));
        }
    }
}
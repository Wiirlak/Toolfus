using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ToolfusDll
{
    public class ClickUtils
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        
        public static IntPtr LParams(int wLow, int wHigh)
        {
            return (IntPtr) ( (short) wHigh << 16 | wLow & ushort.MaxValue);
        }

        public void ClickBackgroundWindowPosition(int pid, int x, int y, char lr)
        {
            IntPtr hWnd = Process.GetProcessById(pid).MainWindowHandle;
            // 513U = WM_LBUTTONDOWN, 516U = WM_RBUTTONDOWN
            SendMessage(hWnd, (lr == 'r' ? 516U : 513U), IntPtr.Zero, LParams(x, y - 30));
            SendMessage(hWnd, (lr == 'r' ? 517U : 514U), IntPtr.Zero, LParams(x, y - 30));
        }

        public void ClickBackgroundWindow(int pid, char lr)
        {
            IntPtr hWnd = Process.GetProcessById(pid).MainWindowHandle;
            // 513U = WM_LBUTTONDOWN, 516U = WM_RBUTTONDOWN
            SendMessage(hWnd, (lr == 'r' ? 516U : 513U), IntPtr.Zero, LParams(0, 0));
            SendMessage(hWnd, (lr == 'r' ? 517U : 514U), IntPtr.Zero, LParams(0, 0));
        }
    }
}
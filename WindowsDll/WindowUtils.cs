using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ToolfusDll
{
    public class WindowUtils
    {
        private const int ALT = 0xA4;
        private const int EXTENDEDKEY = 0x1;
        private const int KEYUP = 0x2;
        private const int SW_SHOW = 5;
        
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
                
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        
        [DllImport("user32.dll", SetLastError=true)]
        static extern bool BringWindowToTop(IntPtr hWnd);
        
        [DllImport("user32.dll")]
        private static extern int AttachThreadInput(int CurrentForegroundThread, int MakeThisThreadForegrouond, bool boolAttach);
        
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        
        public int testfunc()
        {
            return 1;
        }

        public static void ForceForegroundWindow(int pid)
        {
            IntPtr hWnd = Process.GetProcessById(pid).MainWindowHandle;
            uint uforeThread;
            Console.WriteLine("Switching window to " + hWnd + "");
            GetWindowThreadProcessId(GetForegroundWindow(), out uforeThread);
            int foreThread = checked((int)uforeThread);
            int appThread = Thread.CurrentThread.ManagedThreadId;
 
            if (foreThread != appThread)
            {
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);
                SetForegroundWindow(hWnd);
            }
            else
            {
                AttachThreadInput(foreThread, appThread, true);
                BringWindowToTop(hWnd);
                SetForegroundWindow(hWnd);
                ShowWindow(hWnd, SW_SHOW);
                AttachThreadInput(foreThread, appThread, false);
            }
        }
        
        public string GetWindowName(int pid)
        {
            Process process = Process.GetProcessById(pid);
            return process.MainWindowTitle;
        }
        
        

        public uint GetActiveProcess()
        {
            IntPtr hwnd = GetForegroundWindow();

            if (hwnd == null)
                return 0;

            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);

            return pid;
        }
    }
}
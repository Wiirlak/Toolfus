using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ToolfusDll
{
    public class WindowUtils
    {
        private const int SW_SHOW = 5;
        
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

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

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
            int appThread = checked((int)GetCurrentThreadId());

            if (foreThread != appThread)
                AttachThreadInput(appThread, foreThread, true);
            BringWindowToTop(hWnd);
            SetForegroundWindow(hWnd);
            ShowWindow(hWnd, SW_SHOW);
            if (foreThread != appThread)
                AttachThreadInput(appThread, foreThread, false);
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
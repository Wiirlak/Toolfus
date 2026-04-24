using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ToolfusDll
{
    public class WindowUtils
    {
        private const int SW_RESTORE = 9;
        
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
            uint unusedPid;
            Console.WriteLine("Switching window to " + hWnd + "");
            int foreThread = GetWindowThreadProcessId(GetForegroundWindow(), out unusedPid);
            int appThread = checked((int)GetCurrentThreadId());

            if (foreThread != appThread)
                AttachThreadInput(foreThread, appThread, true);
            ShowWindow(hWnd, SW_RESTORE);
            BringWindowToTop(hWnd);
            SetForegroundWindow(hWnd);
            if (foreThread != appThread)
                AttachThreadInput(foreThread, appThread, false);
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
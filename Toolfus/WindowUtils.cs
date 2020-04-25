using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Interop;

namespace Toolfus
{
    public class WindowUtils
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
                
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        
        [DllImport("user32.dll", SetLastError=true)]
        static extern bool BringWindowToTop(IntPtr hWnd);
        
        [DllImport("user32.dll")]
        private static extern int AttachThreadInput(int CurrentForegroundThread, int MakeThisThreadForegrouond, bool boolAttach);
        
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        
        public string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
                return Buff.ToString();
            return null;
        }
        
        public string GetActiveProcessName()
        {
            IntPtr hwnd = GetForegroundWindow();

            // The foreground window can be NULL in certain circumstances, 
            // such as when a window is losing activation.
            if (hwnd == null)
                return "Unknown";

            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);

            foreach (Process p in Process.GetProcesses())
            {
                if (p.Id == pid)
                    return p.ProcessName;
            }

            return "Unknown";
        }
        
        public void changeFocusWindow()
        {
            if (Data.CurrentDofus == null && Data.DofusList.Count > 0)
            {
                Data.CurrentDofus = Data.DofusList[0];
                ForceForegroundWindow(Data.CurrentDofus.MainWindowHandle);
                // SetForegroundWindow(Data.CurrentDofus.MainWindowHandle);
                return;
            }
            Process cur = Data.CurrentDofus;
            int curIndex = Data.DofusList.IndexOf(cur);
            if (curIndex == Data.DofusList.Count - 1)
                curIndex = - 1;
            Process next = Data.DofusList[curIndex + 1];
            
            SetForegroundWindow(next.MainWindowHandle);
            Debug.WriteLine(" okkkkk " + cur.MainWindowTitle);
            Data.CurrentDofus = next;
        }
        
        private static void ForceForegroundWindow(IntPtr hWnd)
        {
            uint uforeThread;
            GetWindowThreadProcessId(GetForegroundWindow(), out uforeThread);
            int foreThread = checked((int)uforeThread);
            int appThread = Thread.CurrentThread.ManagedThreadId;
            const int SW_SHOW = 5;
 
            if (foreThread != appThread)
            {
                AttachThreadInput(foreThread, appThread, true);
                BringWindowToTop(hWnd);
                ShowWindow(hWnd, SW_SHOW);
                AttachThreadInput(foreThread, appThread, false);
            }
            else
            {
                BringWindowToTop(hWnd);
                ShowWindow(hWnd, SW_SHOW);
            }
        }
    }
}
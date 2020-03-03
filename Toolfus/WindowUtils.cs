using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

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
            if (Data.CurrentDofus == null) 
                return;
            Process cur = Data.CurrentDofus;
            int curIndex = Data.DofusList.IndexOf(cur);
            if (curIndex == Data.DofusList.Count - 1)
                curIndex = - 1;
            Process next = Data.DofusList[curIndex + 1];
            SetForegroundWindow(next.MainWindowHandle);
            Debug.WriteLine(" okkkkk " + cur.MainWindowTitle);
            Data.CurrentDofus = next;
        }
    }
}
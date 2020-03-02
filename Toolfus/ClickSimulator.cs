using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Toolfus
{
    public class ClickSimulator
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        
        public static IntPtr LParams(int wLow, int wHigh)
        {
            return (IntPtr) ( (short) wHigh << 16 | wLow & ushort.MaxValue);
        }
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref Data.RECT Rect);
        
        public static void MoveCaracter(char direction)
        {
            foreach (var process in MainWindow.GetCheckedProcess())
            {
                IntPtr handle = process.MainWindowHandle;
                Data.RECT Rect = new Data.RECT();
                if (GetWindowRect(handle, ref Rect))
                {
                    if (direction == Data.KeyMap.Up) 
                        SendClick(process, Rect.right/2,10);
                    else if (direction == Data.KeyMap.Down) 
                        SendClick(process, Rect.right / 2,Rect.bottom - (int) (Rect.bottom *0.15));
                    else if (direction == Data.KeyMap.Left) 
                        SendClick(process, 0,Rect.bottom/2);
                    else if (direction == Data.KeyMap.Right) 
                        SendClick(process, Rect.right-20,Rect.bottom/2);
                }
            }
        }

        private static void SendClick(Process p, int x, int y)
        {
            SendMessage(p.MainWindowHandle, 513U, IntPtr.Zero, 
                LParams(x,y));
            SendMessage(p.MainWindowHandle, 514U, IntPtr.Zero, 
                LParams(x,y));
        }
        
    }
}
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace Toolfus
{
    public class ClickSimulator
    {
        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,  int cbSize);
        
        internal struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        internal struct MOUSEINPUT
        {
            public Int32 X;
            public Int32 Y;
            public UInt32 MouseData;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }

        public static void ClickOnPoint(IntPtr wndHandle , Point clientPoint)
        {
            /*var oldPos = Cursor.Position;

            /// get screen coordinates
            ClientToScreen(wndHandle, ref clientPoint);

            /// set cursor on coords, and press mouse
            Cursor.Position = new System.Drawing.Point((int) clientPoint.X, (int) clientPoint.Y);

            var inputMouseDown = new INPUT();
            inputMouseDown.Type = 0; /// input type mouse
            inputMouseDown.Data.Mouse.Flags = 0x0002; /// left button down

            var inputMouseUp = new INPUT();
            inputMouseUp.Type = 0; /// input type mouse
            inputMouseUp.Data.Mouse.Flags = 0x0004; /// left button up

            var inputs = new INPUT[] { inputMouseDown, inputMouseUp };
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));

            /// return mouse 
            Cursor.Position = oldPos;*/
        }
        
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        
        public static IntPtr LParams(int wLow, int wHigh)
        {
            return (IntPtr) ((int) (short) wHigh << 16 | wLow & (int) ushort.MaxValue);
        }
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref Data.RECT Rect);
        
        public static void MoveCaracter(Char direction)
        {
            foreach (var process in Data.dofus)
            {
                IntPtr handle = process.MainWindowHandle;
                Data.RECT Rect = new Data.RECT();
                if (GetWindowRect(handle, ref Rect))
                {
                    switch (direction)
                    {
                        case 'u':
                            ClickSimulator.SendMessage(process.MainWindowHandle, 513U, IntPtr.Zero, ClickSimulator.LParams(Rect.right/2,10));
                            ClickSimulator.SendMessage(process.MainWindowHandle, 514U, IntPtr.Zero, ClickSimulator.LParams(Rect.right/2,10));
                            Debug.WriteLine("Click up to : \t{0} {1}", Rect.right/2,25);
                            break;
                        case 'd':
                            ClickSimulator.SendMessage(process.MainWindowHandle, 513U, IntPtr.Zero, ClickSimulator.LParams(Rect.right/2,Rect.bottom - 170));
                            ClickSimulator.SendMessage(process.MainWindowHandle, 514U, IntPtr.Zero, ClickSimulator.LParams(Rect.right/2,Rect.bottom - 170));
                            Debug.WriteLine("Click down to : \t{0} {1}", Rect.right/2, Rect.bottom - 55);
                            break;
                        case 'l':
                            ClickSimulator.SendMessage(process.MainWindowHandle, 513U, IntPtr.Zero, ClickSimulator.LParams(0,Rect.bottom/2));
                            ClickSimulator.SendMessage(process.MainWindowHandle, 514U, IntPtr.Zero, ClickSimulator.LParams(0,Rect.bottom/2));
                            Debug.WriteLine("Click left to : \t{0} {1}", 0, Rect.bottom/2);
                            break;
                        case 'r':
                            ClickSimulator.SendMessage(process.MainWindowHandle, 513U, IntPtr.Zero, ClickSimulator.LParams(Rect.right-20,Rect.bottom/2));
                            ClickSimulator.SendMessage(process.MainWindowHandle, 514U, IntPtr.Zero, ClickSimulator.LParams(Rect.right-20,Rect.bottom/2));
                            Debug.WriteLine("Click right to : \t{0} {1}", Rect.right -20, Rect.bottom/2);
                            break;
                    }
                }
            }
        }

    }
}
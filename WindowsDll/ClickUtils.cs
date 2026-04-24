using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ToolfusDll
{
    public class ClickUtils
    {
        // --- P/Invoke ---

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        // --- Structs ---

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int X; public int Y; }

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)] public uint Type;
            [FieldOffset(4)] public MOUSEINPUT Mouse;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint Data;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        // --- Constants ---

        private const uint INPUT_MOUSE          = 0;
        private const uint MOUSEEVENTF_LEFTDOWN  = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP    = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP   = 0x0010;

        // Milliseconds to wait after switching focus before sending input.
        // Unity's input system needs a brief moment to register the new foreground window.
        private const int FocusSwitchDelayMs = 15;

        // --- Helpers ---

        public static IntPtr LParams(int wLow, int wHigh)
        {
            return (IntPtr)((short)wHigh << 16 | wLow & ushort.MaxValue);
        }

        /// <summary>
        /// Force-focus a window using AttachThreadInput so the call succeeds even
        /// when our process is not currently the foreground process.
        /// </summary>
        private static void ForceFocus(IntPtr hWnd)
        {
            uint unusedPid;
            int foreThread = GetWindowThreadProcessId(GetForegroundWindow(), out unusedPid);
            int appThread  = checked((int)GetCurrentThreadId());

            if (foreThread != appThread)
                AttachThreadInput(foreThread, appThread, true);
            BringWindowToTop(hWnd);
            SetForegroundWindow(hWnd);
            if (foreThread != appThread)
                AttachThreadInput(foreThread, appThread, false);
        }

        // --- Public API ---

        /// <summary>
        /// Click a window by temporarily switching focus to it and injecting the
        /// click via SendInput.  This works with Unity / DirectX games that ignore
        /// WM_LBUTTONDOWN sent via SendMessage.
        ///
        /// IMPORTANT: focus is NOT restored by this method.  The caller is
        /// responsible for calling WindowUtils.ForceForegroundWindow on the
        /// original window after all background clicks are done.
        /// </summary>
        public void ClickWindowPositionFocusSwitch(int pid, int x, int y, char lr)
        {
            IntPtr hWnd = Process.GetProcessById(pid).MainWindowHandle;
            Console.WriteLine($"[ClickUtils] ClickWindowPositionFocusSwitch pid={pid} screen=({x},{y}) button={lr}");

            // 1. Switch focus to the target window.
            ForceFocus(hWnd);

            // 2. Move the physical cursor to the target screen position so that
            //    the game's input system sees the cursor inside its window.
            SetCursorPos(x, y);

            // 3. Give the game's input backend time to recognise the new foreground.
            Thread.Sleep(FocusSwitchDelayMs);

            // 4. Inject the click into the hardware-input stream.
            uint downFlag = lr == 'r' ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_LEFTDOWN;
            uint upFlag   = lr == 'r' ? MOUSEEVENTF_RIGHTUP   : MOUSEEVENTF_LEFTUP;

            INPUT[] inputs = new INPUT[2];
            inputs[0] = new INPUT { Type = INPUT_MOUSE, Mouse = new MOUSEINPUT { Flags = downFlag } };
            inputs[1] = new INPUT { Type = INPUT_MOUSE, Mouse = new MOUSEINPUT { Flags = upFlag   } };

            uint sent = SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
            Console.WriteLine($"[ClickUtils] SendInput result: {sent}/2 events injected");
        }

        /// <summary>
        /// Legacy SendMessage-based click.  Kept for reference; does NOT work
        /// with Unity / DirectX games that use Raw Input.
        /// </summary>
        public void ClickBackgroundWindowPosition(int pid, int x, int y, char lr)
        {
            IntPtr hWnd = Process.GetProcessById(pid).MainWindowHandle;
            POINT pt = new POINT { X = x, Y = y };
            ScreenToClient(hWnd, ref pt);
            Console.WriteLine($"[ClickUtils] ClickBackgroundWindowPosition (SendMessage) pid={pid} screen=({x},{y}) client=({pt.X},{pt.Y}) button={lr}");
            uint downMsg = lr == 'r' ? 516U : 513U;
            uint upMsg   = lr == 'r' ? 517U : 514U;
            SendMessage(hWnd, downMsg, IntPtr.Zero, LParams(pt.X, pt.Y));
            SendMessage(hWnd, upMsg,   IntPtr.Zero, LParams(pt.X, pt.Y));
        }
    }
}
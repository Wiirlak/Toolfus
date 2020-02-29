using System;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Windows.Forms;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Toolfus
{
    public class ClickDetector
    {
        private IKeyboardMouseEvents m_GlobalHook;
        private WindowUtils wu;
        private bool follow;
        
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            wu = new WindowUtils();
            m_GlobalHook.MouseClick += GlobalHookMouseClick;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
            follow = false;
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            if (wu.GetActiveProcessName().Equals("Dofus"))
            {
                Debug.WriteLine("KeyPress: \t{0}", e.KeyChar);
                if (e.KeyChar == Data.KeyMap.Follow) follow = !follow;
                else if (follow && Data.KeyMap.InKeyList(e.KeyChar)) ClickSimulator.MoveCaracter(e.KeyChar);
            }

        }

        private void GlobalHookMouseClick(object sender, MouseEventArgs e)
        {
            if(follow && wu.GetActiveProcessName().Equals("Dofus")) DofusClick(e.X, e.Y);
        }

        private void DofusClick(int x, int y)
        {
            Debug.WriteLine("Clic: \tX:{0} | Y:{1}", x, y);
            Debug.WriteLine("Clic: \tX:{0} | Y:{1}", x, y);
            foreach (Process process in MainWindow.GetCheckedProcess())
            {
                ClickSimulator.SendMessage(process.MainWindowHandle, 513U, IntPtr.Zero,
                    ClickSimulator.LParams(x, y - 30));
                ClickSimulator.SendMessage(process.MainWindowHandle, 514U, IntPtr.Zero,
                    ClickSimulator.LParams(x, y - 30));
            }
        }
        
        public void Unsubscribe()
        {
            //m_GlobalHook.KeyPress -= GlobalHookKeyPress;
            m_GlobalHook.MouseClick -= GlobalHookMouseClick;
            m_GlobalHook.Dispose();
        }
    }
}
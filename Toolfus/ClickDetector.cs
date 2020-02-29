using System;
using System.Collections.Generic;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Toolfus
{
    public class ClickDetector
    {
        const uint WM_KEYDOWN = 0x100;
        const uint WM_KEYUP = 0x101;
        private IKeyboardMouseEvents m_GlobalHook;
        private WindowUtils wu;
        private bool sub = false;
        
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            wu = new WindowUtils();

            m_GlobalHook.MouseUpExt += GlobalHookMouseUpExt;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
            sub = true;
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            if (wu.GetActiveProcessName().Equals("Dofus"))
            {
                Debug.WriteLine("KeyPress: \t{0}", e.KeyChar);
                if (e.KeyChar == 'k' || e.KeyChar == 'K')
                {
                    if (sub)
                        this.Unsubscribe();
                    else
                        this.Subscribe();
                }
                else if (e.KeyChar == '8')
                {
                    ClickSimulator.MoveCaracter('u');
                }
                else if (e.KeyChar == '5')
                {
                    ClickSimulator.MoveCaracter('d');
                }
                else if (e.KeyChar == '4')
                {
                    ClickSimulator.MoveCaracter('l');
                }
                else if (e.KeyChar == '6')
                {
                    ClickSimulator.MoveCaracter('r');
                }
                else
                {
                    // foreach (Process process in Data.dofus)
                    // {
                    //     MainWindow.SendMessage(process.MainWindowHandle, WM_KEYDOWN, IntPtr.Zero, (IntPtr)e.KeyChar);
                    //     MainWindow.SendMessage(process.MainWindowHandle, WM_KEYUP, IntPtr.Zero, (IntPtr)e.KeyChar);
                    // }
                }
            }

        }

        private void GlobalHookMouseUpExt(object sender, MouseEventExtArgs e)
        {
            if (wu.GetActiveProcessName().Equals("Dofus"))
            {
                Debug.WriteLine("Clic: \tX:{0} | Y:{1}", e.X, e.Y);
                Debug.WriteLine("Clic: \tX:{0} | Y:{1}", e.X, e.Y);
                // Debug.WriteLine("Window : " + wu.GetActiveWindowTitle() );
                // Debug.WriteLine("Process : " + wu.GetActiveProcessName() );
                foreach (Process process in Data.dofus)
                {
                    ClickSimulator.SendMessage(process.MainWindowHandle, 513U, IntPtr.Zero,
                        ClickSimulator.LParams(e.X, e.Y));
                    ClickSimulator.SendMessage(process.MainWindowHandle, 514U, IntPtr.Zero,
                        ClickSimulator.LParams(e.X, e.Y));
                }
            }

        }

        public void DofusProcessClick(List<Process> process, Point clic)
        {
            foreach (var p in process)
            {
                Debug.WriteLine("clic on :"  + p.MainWindowTitle);
                ClickSimulator.ClickOnPoint(p.Handle, clic);
            }
        }
        
        public void Unsubscribe()
        {
            //m_GlobalHook.KeyPress -= GlobalHookKeyPress;
            m_GlobalHook.MouseUpExt -= GlobalHookMouseUpExt;
            m_GlobalHook.Dispose();
            sub = false;
        }
    }
}
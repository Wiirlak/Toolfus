using System;
using System.Collections.Generic;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;

namespace Toolfus
{
    public class ClickDetector
    {
        private IKeyboardMouseEvents m_GlobalHook;
        private WindowUtils wu;
        private bool sub = false;
        
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            wu = new WindowUtils();

            m_GlobalHook.MouseClick += GlobalHookMouseClick;
            m_GlobalHook.MouseDown += GlobalHookMouseDown;
            m_GlobalHook.MouseUp += GlobalHookMouseUp;
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
            }

        }

        private void GlobalHookMouseClick(object sender, MouseEventArgs e)
        {
            if (wu.GetActiveProcessName().Equals("Dofus"))
            {
                Debug.WriteLine("Clic: \tX:{0} | Y:{1}", e.X, e.Y);
                Debug.WriteLine("Clic: \tX:{0} | Y:{1}", e.X, e.Y);
                // Debug.WriteLine("Window : " + wu.GetActiveWindowTitle() );
                // Debug.WriteLine("Process : " + wu.GetActiveProcessName() );
                foreach (Process process in MainWindow.GetCheckedProcess())
                {
                    ClickSimulator.SendMessage(process.MainWindowHandle, 513U, IntPtr.Zero,
                        ClickSimulator.LParams(e.X, e.Y - 30));
                    ClickSimulator.SendMessage(process.MainWindowHandle, 514U, IntPtr.Zero,
                        ClickSimulator.LParams(e.X, e.Y - 30));
                }
            }

        }
        
        private void GlobalHookMouseDown(object sender, MouseEventArgs e)
        {
            if (wu.GetActiveProcessName().Equals("Dofus"))
            {
                Debug.WriteLine("Clic: \tX:{0} | Y:{1}", e.X, e.Y);
                Debug.WriteLine("Clic: \tX:{0} | Y:{1}", e.X, e.Y);
                // Debug.WriteLine("Window : " + wu.GetActiveWindowTitle() );
                // Debug.WriteLine("Process : " + wu.GetActiveProcessName() );
                foreach (Process process in MainWindow.GetCheckedProcess())
                {
                    ClickSimulator.SendMessage(process.MainWindowHandle, 513U, IntPtr.Zero,
                        ClickSimulator.LParams(e.X, e.Y - 30));
                }
            }

        }
        
        private void GlobalHookMouseUp(object sender, MouseEventArgs e)
        {
            if (wu.GetActiveProcessName().Equals("Dofus"))
            {
                Debug.WriteLine("Clic: \tX:{0} | Y:{1}", e.X, e.Y);
                Debug.WriteLine("Clic: \tX:{0} | Y:{1}", e.X, e.Y);
                // Debug.WriteLine("Window : " + wu.GetActiveWindowTitle() );
                // Debug.WriteLine("Process : " + wu.GetActiveProcessName() );
                foreach (Process process in MainWindow.GetCheckedProcess())
                {
                    ClickSimulator.SendMessage(process.MainWindowHandle, 514U, IntPtr.Zero,
                        ClickSimulator.LParams(e.X, e.Y - 30));
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
            m_GlobalHook.MouseClick -= GlobalHookMouseClick;
            m_GlobalHook.MouseDown -= GlobalHookMouseDown;
            m_GlobalHook.MouseUp -= GlobalHookMouseUp;
            m_GlobalHook.Dispose();
            sub = false;
        }
    }
}
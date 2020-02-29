using System;
using System.Collections.Generic;
using System.Configuration;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

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
                if (e.KeyChar == ConfigurationManager.AppSettings.GetValues("KeyFollow")[0][0])
                {
                    if (sub)
                        this.Unsubscribe();
                    else
                        this.Subscribe();
                }
                else if (e.KeyChar == ConfigurationManager.AppSettings.GetValues("KeyUp")[0][0])
                {
                    ClickSimulator.MoveCaracter('u');
                }
                else if (e.KeyChar == ConfigurationManager.AppSettings.GetValues("KeyDown")[0][0])
                {
                    ClickSimulator.MoveCaracter('d');
                }
                else if (e.KeyChar == ConfigurationManager.AppSettings.GetValues("KeyLeft")[0][0])
                {
                    ClickSimulator.MoveCaracter('l');
                }
                else if (e.KeyChar == ConfigurationManager.AppSettings.GetValues("KeyRight")[0][0])
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
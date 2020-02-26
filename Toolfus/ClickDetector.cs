using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Windows.Forms;

namespace Toolfus
{
    public class ClickDetector
    {
        private IKeyboardMouseEvents m_GlobalHook;
        private WindowUtils wu;
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            wu = new WindowUtils();

            m_GlobalHook.MouseUpExt += GlobalHookMouseUpExt;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            Debug.WriteLine("KeyPress: \t{0}", e.KeyChar);
        }

        private void GlobalHookMouseUpExt(object sender, MouseEventExtArgs e)
        {
            if(wu.GetActiveProcessName().Equals("Dofus"))
                Debug.WriteLine("MouseDown: \tX:{0} | Y:{1}", e.X, e.Y);
            // Debug.WriteLine("Window : " + wu.GetActiveWindowTitle() );
            // Debug.WriteLine("Process : " + wu.GetActiveProcessName() );

            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
        }
        
        


        public void Unsubscribe()
        {
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;
            m_GlobalHook.MouseUpExt -= GlobalHookMouseUpExt;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }
    }
}
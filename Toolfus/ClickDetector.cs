using System;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Windows.Forms;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Toolfus
{
    public class ClickDetector
    {
        private IKeyboardMouseEvents _globalHook;
        private WindowUtils _wUtils;
        private bool _follow;
        private System.Media.SoundPlayer _player;

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            _globalHook = Hook.GlobalEvents();
            _wUtils = new WindowUtils();
            _globalHook.MouseClick += GlobalHookMouseClick;
            _globalHook.KeyPress += GlobalHookKeyPress;
            _follow = false;
            _player = new System.Media.SoundPlayer();
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            if (_wUtils.GetActiveProcessName().Equals("Dofus"))
            {
                Debug.WriteLine("KeyPress: \t{0}", e.KeyChar);
                if (e.KeyChar == Data.KeyMap.Follow)
                {
                    _follow = !_follow;
                    _player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "/sounds/" + (_follow ? "open.wav" : "close.wav");
                    _player.Play();
                }
                else if (_follow && Data.KeyMap.InMoveKeyList(e.KeyChar)) 
                    ClickSimulator.MoveCaracter(e.KeyChar);
            }
            if (e.KeyChar == Data.KeyMap.Switch)
                _wUtils.changeFocusWindow();

        }

        private void GlobalHookMouseClick(object sender, MouseEventArgs e)
        {
            if (_follow && _wUtils.GetActiveProcessName().Equals("Dofus"))
            {
                if (e.Button == MouseButtons.Left)
                    DofusClick(e.X, e.Y, 'l');
                if (e.Button == MouseButtons.Right)
                    DofusClick(e.X, e.Y, 'r');
            }
        }

        private void DofusClick(int x, int y, char lr)
        {
            Debug.WriteLine("Clic: \tX:{0} | Y:{1}", x, y);
            Debug.WriteLine("Clic: \tX:{0} | Y:{1}", x, y);
            foreach (Process process in MainWindow.GetCheckedProcess())
            {
                ClickSimulator.SendMessage(process.MainWindowHandle, (lr=='r'?516U:513U), IntPtr.Zero,
                    ClickSimulator.LParams(x, y - 30));
                ClickSimulator.SendMessage(process.MainWindowHandle, (lr=='r'?517U:514U), IntPtr.Zero,
                    ClickSimulator.LParams(x, y - 30));
            }
        }
        
        public void Unsubscribe()
        {
            _globalHook.KeyPress -= GlobalHookKeyPress;
            _globalHook.MouseClick -= GlobalHookMouseClick;
            _globalHook.Dispose();
        }
    }
}
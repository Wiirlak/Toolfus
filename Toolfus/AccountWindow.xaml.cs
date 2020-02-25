using System.Diagnostics;
using System.Windows;

namespace Toolfus
{
    public partial class AccountWindow : Window
    {
        public AccountWindow()
        {
            InitializeComponent();
        }
        
        private void ButtonStartDofus_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\Users\\Bastien\\AppData\\Local\\Programs\\zaap\\Ankama Launcher.exe");
        } 
            
            
    }
}
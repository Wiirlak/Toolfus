using System.Windows;
using WpfApplication1;

namespace Toolfus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonAccount_OnClick(object sender, RoutedEventArgs e)
        {
            AccountWindow acc = new AccountWindow();
            acc.Show();
        }

        private void ButtonConnect_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void ButtonKeymap_OnClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new  KeymapPage();
        }

        private void ButtonChat_OnClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new  ChatPage();
        }
    }
}
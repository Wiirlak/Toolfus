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

        private void ButtonOptions_OnClick(object sender, RoutedEventArgs e)
        {
            OptionsWindow acc = new OptionsWindow();
            acc.Show();
        }

        private void ButtonChat_OnClick(object sender, RoutedEventArgs e)
        {
            ChatWindow acc = new ChatWindow();
            acc.Show();
        }
    }
}
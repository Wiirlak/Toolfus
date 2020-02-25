using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Toolfus
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        List<Process> dofus = new List<Process>();
        ClickDetector clicker = new ClickDetector();

        public MainWindow()
        {
            InitializeComponent();
            clicker.Subscribe();
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.MainWindowTitle.Contains("Dofus"))
                {
                    // dofus.Add("Process: "+process.ProcessName +" ID: "+process.Id+" Window title: "+process.MainWindowTitle+"");
                    Border border = new Border();
                    CheckBox checkBox =  new CheckBox();
                    checkBox.Margin = new Thickness { Left = 5, Top = 0, Right = 0, Bottom = 0 };
                    checkBox.Content = process.MainWindowTitle;
                    border.Child = checkBox;
                    dofus.Add(process);
                    // dofusProcess.Children.Add(border);
                }
            }
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
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            clicker.Unsubscribe();
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
    
}
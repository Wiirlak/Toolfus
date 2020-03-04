using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Toolfus
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        ClickDetector clicker = new ClickDetector();
        public static ListView DofusProcessList;

        public MainWindow()
        {
            InitializeComponent();
            clicker.Subscribe();
            getWindows();
            DofusProcessList = DofusProcess;
        }

        private void getWindows()
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.ProcessName.Equals("Dofus"))
                {
                    Data.DofusList.Add(process);
                    
                    // dofus.Add("Process: "+process.ProcessName +" ID: "+process.Id+" Window title: "+process.MainWindowTitle+"");
                    CheckBox checkBox =  new CheckBox();
                    checkBox.Margin = new Thickness { Left = 5, Top = 0, Right = 0, Bottom = 0 };
                    checkBox.Content = process.MainWindowTitle;
                    DofusProcess.Items.Add(checkBox);
                    checkBox.AddHandler(MouseDownEvent, new MouseButtonEventHandler(DofusProcess_Pop), true);
                    checkBox.AddHandler(DropEvent, new DragEventHandler(DofusProcess_Drop), true);
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
        
        private void ButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Data.DofusList.Clear();
            DofusProcess.Items.Clear();
            getWindows();
        }
        
        /// <summary>
        /// CloseButton_Clicked
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            clicker.Unsubscribe();
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        
        public static List<Process> GetCheckedProcess()
        {
            List<Process> result = new List<Process>();
            foreach (CheckBox cb in  DofusProcessList.Items)
            {
                if((bool)cb.IsChecked)
                    result.Add(FindProcess(cb.Content.ToString()));
            }

            return result;
        }

        public  static Process FindProcess(String processName)
        {
            foreach (var process in Data.DofusList)
            {
                if (process.MainWindowTitle == processName)
                    return process;
            }

            return null;
        }

        private void DofusProcess_Pop(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is CheckBox)
            {
                CheckBox c = (CheckBox) e.Source;
                DataObject dataObj = new DataObject(c);
                DragDrop.DoDragDrop(c, dataObj, DragDropEffects.Move);
            }
        }

        private void DofusProcess_Drop(object sender, DragEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox dropped = (CheckBox) e.Data.GetData(typeof(CheckBox));
                CheckBox target = (CheckBox) sender;
                
                int droppedIndex = DofusProcessList.Items.IndexOf(dropped);
                int targetIndex = DofusProcessList.Items.IndexOf(target);
                Debug.WriteLine(target.Content + " <-> " + dropped.Content);
                Process pdropped = Data.DofusList[droppedIndex];

                if (dropped.Content == target.Content)
                {
                    dropped.IsChecked = !dropped.IsChecked;
                    return;
                }
                if (droppedIndex < targetIndex)
                {
                    DofusProcessList.Items.RemoveAt(droppedIndex);
                    DofusProcessList.Items.Insert(targetIndex, dropped);
                    Data.DofusList.RemoveAt(droppedIndex);
                    Data.DofusList.Insert(targetIndex, pdropped);
                }else
                {
                    int remIdx = droppedIndex;
                    if (DofusProcessList.Items.Count + 1 > remIdx)
                    {
                        DofusProcessList.Items.RemoveAt(remIdx);
                        DofusProcessList.Items.Insert(targetIndex, dropped);
                        Data.DofusList.RemoveAt(remIdx);
                        Data.DofusList.Insert(targetIndex, pdropped);
                    }
                }
                Data.CurrentDofus = null;
            }
        }
    }
    
}
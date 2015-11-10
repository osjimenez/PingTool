using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using static System.Console;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Threading;

namespace PingTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        bool running;
        private void Console(string message)
        {
            WriteLine(message);
        }
        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            Ping Pings = new Ping();
            int timeout = 500;
            int interval = 1000;
            int errors = 0;
            int errorLimit = 2;
            running = true;
            RunButton.Visibility = Visibility.Collapsed;
            StopButton.Visibility = Visibility.Visible;
            StatusTextBlock.Visibility = Visibility.Visible;
            while (running)
            {
                try {
                    var res = await Pings.SendPingAsync("8.8.8.8", timeout);
                    if (res.Status == IPStatus.Success)
                    {
                        Console("Success, time: " + res.RoundtripTime + " ms");
                        SetIcon("Green");
                        StatusTextBlock.Text = "Success";
                        StatusTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                        await Task.Delay(interval - (int)res.RoundtripTime);
                    }
                    else
                    {
                        errors++;
                        if (errors >= errorLimit)
                        {
                            Console("Error");
                            SetIcon("Red");
                            StatusTextBlock.Text = "Error";
                            StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                        }
                    }
                }
                catch
                {
                    errors++;
                    if (errors >= errorLimit)
                    {
                        Console("Error");
                        SetIcon("Red");
                        StatusTextBlock.Text = "Error";
                        StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                    }
                    await Task.Delay(interval);
                }
            }
            SetIcon("White");
            RunButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Collapsed;
            StatusTextBlock.Visibility = Visibility.Collapsed;
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            running = false;
        }
        private void SetIcon(string iconName)
        {
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/PingTool;component/" + iconName + ".ico")).Stream;
            NotifyIcon.Icon = new Icon(iconStream);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
#if DEBUG
            Application.Current.Shutdown();
#endif
            Visibility = Visibility.Collapsed;
            e.Cancel = true;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Show_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Visible;
        }
        public ActionCommand ShowCommand { get { return new ActionCommand(() => Visibility = Visibility.Visible); } }
    }
    public class ActionCommand : ICommand
    {
        public ActionCommand(Action action) { this.action = action; }
        Action action;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            action();
        }
    }
}

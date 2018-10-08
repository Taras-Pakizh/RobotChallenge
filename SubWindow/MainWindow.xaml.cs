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

using Robot.Common;

namespace SubWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler StartLogging;
        public event EventHandler StopLogging;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddMessage(object sender, LogEventArgs e)
        {
            MyLogger.Items.Add(e.Message + " " + e.Owner + " " + e.Priority);
        }

        public void AddRobotMessage(object sender, RobotLogicEventArgs e)
        {
            MyLogger.Items.Add(e.ToString());
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopLogging?.Invoke(this, new EventArgs());
        }

        private void Start_Click_1(object sender, RoutedEventArgs e)
        {
            StartLogging?.Invoke(this, new EventArgs());
        }
    }
}

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

namespace TCPPrac
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, RoutedEventArgs e)
        {
            var serverView = new Views.ServerView();
            serverView.Show();
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            var clientView = new Views.ClientView();
            clientView.Show();
        }
    }
}

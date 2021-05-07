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

namespace Warehouse
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserControl1 userControl1 = new UserControl1();
        UserControl2 userControl2 = new UserControl2();
        public MainWindow()
        {
            InitializeComponent();
            
            Framef.Content = userControl1;
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            //UserControl1 userControl1 = new UserControl1();
            Framef.Content = userControl1;
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            
            Framef.Content = userControl2;
        }
    }
}

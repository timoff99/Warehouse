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
using System.Windows.Shapes;
using Warehouse.Models;

namespace Warehouse.Views.User
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        UserWelcomePage UserWelcomePage;
        UserAllStuffRedactorPage UserAllStuffRedactorPage = new UserAllStuffRedactorPage();
        UserMapPage UserMapPage = new UserMapPage();
        public UserWindow()
        {
            InitializeComponent();
        }
        public UserWindow(Users user)
        {
            InitializeComponent();
            UserWelcomePage = new UserWelcomePage(user);
            Framef.Content = UserWelcomePage;
        }

        private void OpenUserWelcomPage_Click(object sender, RoutedEventArgs e)
        {
            Framef.Content = UserWelcomePage;
        }

        private void OpenUserAllStaffRedactorPage_Click(object sender, RoutedEventArgs e)
        {
            Framef.Content = UserAllStuffRedactorPage;
        }

        private void OpenMapPage_Click(object sender, RoutedEventArgs e)
        {
            Framef.Content = UserMapPage;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();

        }
    }
}

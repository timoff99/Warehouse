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

namespace Warehouse.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        AdminWelcomePage AdminWelcomePage;
        AdminAllStaffRedactorPage AdminAllStaffRedactorPage = new AdminAllStaffRedactorPage();
        public AdminWindow()
        {
            InitializeComponent();
            AdminWelcomePage AdminWelcomePage = new AdminWelcomePage();
            Framef.Content = AdminWelcomePage;
        }
        public AdminWindow(Users user)
        {
            InitializeComponent();
            AdminWelcomePage = new AdminWelcomePage(user);
            Framef.Content = AdminWelcomePage;
        }

        private void OpenAdminWelcomPage_Click(object sender, RoutedEventArgs e)
        {
            
            Framef.Content = AdminWelcomePage;
        }

        private void OpenAdminAllStaffRedactorPage_Click(object sender, RoutedEventArgs e)
        {

            Framef.Content = AdminAllStaffRedactorPage;
        }
    }
}

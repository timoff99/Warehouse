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
using Warehouse.Models;

namespace Warehouse.Views.User
{
    /// <summary>
    /// Логика взаимодействия для UserWelcomePage.xaml
    /// </summary>
    public partial class UserWelcomePage : UserControl
    {
        public UserWelcomePage()
        {
            InitializeComponent();
        }
        public UserWelcomePage(Users user)
        {
            InitializeComponent();
            DataContext = this;

            using (DatabaseEntities ctx = new DatabaseEntities())
            {
                userName.Text = user.Name;
            }
        }
    }
}

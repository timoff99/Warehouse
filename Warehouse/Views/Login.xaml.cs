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
using Warehouse.Views;

namespace Warehouse
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            Close();
            registration.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using(var ctx = new DatabaseEntities())
                {
                    Users tryUserIdentify = new Users()
                    {
                        Email = LoginEmailTextBox.Text.ToString().Trim(),
                        Password = Security.Encrypt(LoginPasswordBox.Password.ToString().Trim())
                    };
                    Users user = ctx.Users
                        .Where(x => x.Password == tryUserIdentify.Password && x.Email == tryUserIdentify.Email).FirstOrDefault();
                    
                    if(user != null)
                    {
                        MessageBox.Show($"Пользователь вошел.", "Успешно добавлен", MessageBoxButton.OK, MessageBoxImage.Information);
                        AdminWindow adminWindow = new AdminWindow(user);// поменять на user или admin Window
                        Close();
                        adminWindow.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Информация об ошибке: {ex.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

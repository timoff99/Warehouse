using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        DatabaseEntities ctx;
        public Registration()
        {
            InitializeComponent();
        }

        private void EnterLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {         
            try
            {
                using (ctx = new DatabaseEntities())
                {

                    
                    Users user = new Users()
                    {
                        Name = NameRegisterTextBox.Text.ToString().Trim(),
                        Email = EmailRegisterTextBox.Text.ToString().Trim(),
                        Password = Security.Encrypt(PasRegisterTextBox.Text.ToString().Trim())
                    };
                    
                    ctx.Users.Add(user);
                    ctx.Entry(user).State = EntityState.Added;
                    ctx.SaveChanges();
                    MessageBox.Show($"Пользователь зарегестрирован.","Успешно добавлен",MessageBoxButton.OK,MessageBoxImage.Information);
                    Login login = new Login();
                    login.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Информация об ошибке: {ex.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterValidation_TextChangedEvent(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Подсветка некорректных полей
                TextBox tb = sender as TextBox;
                Validation.CanUserPressRegButton(tb);

                
                    if (Validation.CanUserPressRegButton(NameRegisterTextBox)
                    && Validation.CanUserPressRegButton(EmailRegisterTextBox)
                    && Validation.CanUserPressRegButton(PasRegisterTextBox))
                        RegistrationButton.IsEnabled = true;

                    else
                        RegistrationButton.IsEnabled = false;
                
            }

            catch (Exception exteption)
            {
                MessageBox.Show($"Информация об ошибке: {exteption.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        
    }
}

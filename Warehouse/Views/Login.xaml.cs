using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
using Warehouse.Views.Admin;
using Warehouse.Views.User;

namespace Warehouse
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DatabaseEntities db = new DatabaseEntities();
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

        //Вход
        private void ButtonEntry_Click(object sender, RoutedEventArgs e)
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
                        if(user.IsAdmin == true)
                        { 
                        MessageBox.Show($"Админ вошел.", "Успешно добавлен", MessageBoxButton.OK, MessageBoxImage.Information);
                        AdminWindow adminWindow = new AdminWindow(user);
                        Close();
                        adminWindow.ShowDialog();
                        }
                        if(user.IsAdmin == false)
                        {
                            MessageBox.Show($"Пользователь вошел.", "Успешно добавлен", MessageBoxButton.OK, MessageBoxImage.Information);
                            UserWindow userWindow = new UserWindow(user);
                            Close();
                            userWindow.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Информация об ошибке: {ex.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Восстановление пароля
        private async void ButtonRestorePassword_Click(object sender, RoutedEventArgs e)
        {
            var email = LoginEmailTextBox.Text;
            
            using (db = new DatabaseEntities())
            {
                var currentUser = db.Users.Where(x => x.Email == email).FirstOrDefault();
               if(currentUser != null)
                { 
              await Task.Run(() =>
                {
                    MailAddress from = new MailAddress("qq1262245@gmail.com", "PartShop");
                    MailAddress to = new MailAddress(email);
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "Test";
                    message.Body = $"Здравстуйте {currentUser.Name} \nВаш пароль воссановлен { Security.Decrypt(currentUser.Password.ToString())}";
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential("qq1262245@gmail.com", "hfukmbejrhgalhcv");
                    smtp.EnableSsl = true;
                    smtp.SendMailAsync(message);
                    MessageBox.Show($"Пароль отправлен на указанный при регистрации email", "Успешно отправлен", MessageBoxButton.OK, MessageBoxImage.Information);

                });
                }
                else
                {
                    MessageBox.Show($" email введен не правильно", "Успешно отправлен", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }
    }
}

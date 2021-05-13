using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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

namespace Warehouse.Views.Add
{
    /// <summary>
    /// Логика взаимодействия для AddUsersWindow.xaml
    /// </summary>
    public partial class AddUsersWindow : Window
    {
        DatabaseEntities db = new DatabaseEntities();
        public AddUsersWindow()
        {
            InitializeComponent();
        }
        
        public void FillComboBox(ComboBox cb)
        {
            using (db = new DatabaseEntities())
            {
                //«Ответственное лицо»
                if (cb == IsAdminComboBox)
                {
                    bool Ttrue = true;
                    bool Ffalse = false;
                    BindingList<string> isAdmin = new BindingList<string>();

                        isAdmin.Add(Ffalse.ToString());
                        isAdmin.Add(Ttrue.ToString());
                    
                    cb.ItemsSource = isAdmin;
                }
            }
        }
        private void _Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox(IsAdminComboBox);

        }
        private void GoodsInfoValidation_TextChangedEvent(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Подсветка некорректных полей
                TextBox tb = sender as TextBox;
                Validation.CanUserPressRegButton(tb);


                if (Validation.CanUserPressRegButton(NameRegisterTextBox)
                && Validation.CanUserPressRegButton(EmailRegisterTextBox)
                && Validation.CanUserPressRegButton(PasRegisterTextBox))
                    ButtonAdd.IsEnabled = true;

                else
                    ButtonAdd.IsEnabled = false;

            }

            catch (Exception exteption)
            {
                MessageBox.Show($"Информация об ошибке: {exteption.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void AddNewItemsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonAdd.IsEnabled == true)
            {
                using (db = new DatabaseEntities())
                {
                    Users AddUser = new Users();

                    AddUser.Name = NameRegisterTextBox.Text.ToString();
                    AddUser.Email = EmailRegisterTextBox.Text.ToString();
                    AddUser.Password = Security.Encrypt(PasRegisterTextBox.Text.ToString().Trim());
                    AddUser.IsAdmin = bool.Parse(IsAdminComboBox.Text);
                    db.Entry(AddUser).State = EntityState.Added;
                    db.SaveChanges();
                    Close();

                }
            }
        }
    }
}

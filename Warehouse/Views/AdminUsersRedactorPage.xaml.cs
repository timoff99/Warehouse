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
using Warehouse.Views.Add;

namespace Warehouse.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminUsersRedactorPage.xaml
    /// </summary>
    public partial class AdminUsersRedactorPage : UserControl
    {
        DatabaseEntities db = new DatabaseEntities();
        public AdminUsersRedactorPage()
        {
            InitializeComponent();
        }
        public void FillDataGrid()
        {
            using (db = new DatabaseEntities())
            {
                var allItems = db.Users;
                BindingList<Users> users = new BindingList<Users>();
                foreach (Users s in allItems)
                {

                    users.Add(s);
                }
                UsersDataGrid.ItemsSource = users;
            }
        }
        private void TextBoxItemsSearchGotFocus_Event(object sender, RoutedEventArgs e)
        {
            textBoxItemsSearch.Text = "";
        }

        private void TextBoxChangedSearchItems_Event(object sender, KeyEventArgs e)
        {
            using (db = new DatabaseEntities()) // в фильтре многовато условий подчистить 
            {
                var filter = db.Users.Where(x => x.Name.Contains(NameRegisterTextBox.Text) ||
                                                x.Email.ToString().Contains(EmailRegisterTextBox.Text) ||
                                                x.Password.ToString().Contains(PasRegisterTextBox.Text) ||
                                                x.IsAdmin.ToString().Contains(IsAdminComboBox.Text)
                                                );
                BindingList<Users> user = new BindingList<Users>();
                foreach (Users item in filter)
                {
                    user.Add(item);

                }
                UsersDataGrid.ItemsSource = user;
            }
        }
        private void ButtonResetSearchItems_Click(object sender, RoutedEventArgs e)
        {
            textBoxItemsSearch.Text = "Поиск...";
            FillDataGrid();
        }
        public void FillComboBox(ComboBox cb)
        {
            using (db = new DatabaseEntities())
            {
                //«Ответственное лицо»
                if (cb == IsAdminComboBox)
                {
                    var allEmployees = db.Users;
                    BindingList<string> isAdmin = new BindingList<string>();
                    foreach (var item in allEmployees)
                    {
                        isAdmin.Add(item.IsAdmin.ToString());
                    }
                    cb.ItemsSource = isAdmin;
                }
            }
        }
        private void _Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox(IsAdminComboBox);
            FillDataGrid();
        }

        private void GoodsInfoValidation_TextChangedEvent(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Подсветка некорректных полей
                TextBox tb = sender as TextBox;
                Validation.CanUserPressRegButton(tb);


                if (Validation.CanUserPressRegButton(NameRegisterTextBox)
                && Validation.CanUserPressRegButton(EmailRegisterTextBox))
                    ButtonUpdateGoodsInfo.IsEnabled = true;

                else
                    ButtonUpdateGoodsInfo.IsEnabled = false;

            }

            catch (Exception exteption)
            {
                MessageBox.Show($"Информация об ошибке: {exteption.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ButtonUpdateGoodsInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateAndSaveChanges("Users");
        }
        public void UpdateAndSaveChanges(string WhatSave)
        {
            try
            {
                using (db = new DatabaseEntities())
                {
                    //Логика обновления СОТРУДНИКА
                    if (WhatSave == "Users")
                    {
                        if (UsersDataGrid.SelectedCells.Count == 4)
                        {
                            Users currentUser = UsersDataGrid.SelectedItem as Users;
                            Users updatedUser = new Users();
                            //поиск обновляемого элемента в контексте по ID (Primary key)
                            updatedUser = db.Users.Where(x => x.Id == currentUser.Id).FirstOrDefault();
                            updatedUser.Name = NameRegisterTextBox.Text.ToString();
                            updatedUser.Email = EmailRegisterTextBox.Text.ToString();
                            updatedUser.Password = Security.Encrypt(PasRegisterTextBox.Text.ToString().Trim());
                            updatedUser.IsAdmin = bool.Parse(IsAdminComboBox.Text);
                            db.Entry(updatedUser).State = EntityState.Modified;
                           
                            db.SaveChanges();

                            FillDataGrid();

                        }
                    }
                }
            }
            catch (Exception exteption)
            {
                MessageBox.Show($"Информация об ошибке: {exteption.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            using (db = new DatabaseEntities())
            {
                Users selectedItem = UsersDataGrid.SelectedItem as Users;

                var item = db.Users.Where(x => x.Id == selectedItem.Id).FirstOrDefault();
                db.Users.Remove(item);
                db.Entry(item).State = EntityState.Deleted;
                db.SaveChanges();

                FillDataGrid();
            }
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            AddUsersWindow addUsersWindow = new AddUsersWindow();
            addUsersWindow.ShowDialog();
            FillDataGrid();
        }
    }
}

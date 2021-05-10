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
using System.Windows.Shapes;
using Warehouse.Models;
using Warehouse;

namespace Warehouse.Views.Add
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeesWindow.xaml
    /// </summary>
    public partial class AddEmployeesWindow : Window
    {
        DatabaseEntities db = new DatabaseEntities();
        public AddEmployeesWindow()
        {
            InitializeComponent();
        }
        
        private void AddNewItemsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonAdd.IsEnabled == true)
            {
                using (db = new DatabaseEntities())
                {
                    Employees AddEmployee = new Employees();

                    //Имя
                    AddEmployee.Name = textBoxEmployeeName.Text.ToString();

                    //Телефон
                    AddEmployee.Phone = textBoxEmployeePhone.Text;

                    //Email
                    AddEmployee.Email = textBoxEmployeeEmail.Text;


                    db.Entry(AddEmployee).State = EntityState.Added;
                    db.SaveChanges();
                    Close();

                }
            }

        }

        private void Validation_TextChangedEvent(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox tb = sender as TextBox;
                Validation.TextChanged(tb);
                if (Validation.TextChanged(textBoxEmployeeName) && Validation.TextChanged(textBoxEmployeePhone)
                    && Validation.TextChanged(textBoxEmployeeEmail))
                    ButtonAdd.IsEnabled = true;
                else
                    ButtonAdd.IsEnabled = false;
            }
            catch (Exception exteption)
            {
                MessageBox.Show($"Информация об ошибке: {exteption.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

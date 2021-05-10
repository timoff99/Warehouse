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
    /// Логика взаимодействия для AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        DatabaseEntities db = new DatabaseEntities();
        public AddItemWindow()
        {
            InitializeComponent();
        }

        public void FillComboBox(ComboBox cb)
        {
            using (db = new DatabaseEntities())
            {
                //«Ответственное лицо»
                if (cb == ComboBoxItemsResponsibleEmployee)
                {
                    var allEmployees = db.Employees;
                    BindingList<string> employees = new BindingList<string>();
                    foreach (var item in allEmployees)
                    {
                        employees.Add(item.Name);
                    }
                    cb.ItemsSource = employees;
                }

                //«Категория»
                if (cb == ComboBoxItemsCategory)
                {
                    var allCategories = db.Categories;
                    BindingList<string> categories = new BindingList<string>();
                    foreach (var item in allCategories)
                    {
                        categories.Add(item.Name);
                    }
                    cb.ItemsSource = categories;

                }
            }
        }
        private void _Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox(ComboBoxItemsResponsibleEmployee);
            FillComboBox(ComboBoxItemsCategory);
        }
        private void AddNewItemsButton_Click(object sender, RoutedEventArgs e)
        {
            if(ButtonAdd.IsEnabled == true)
            { 
            using (db = new DatabaseEntities())
            {
                StaffItems AddItem = new StaffItems();

                //Название
                AddItem.ItemName = textBoxStaffItemsName.Text.ToString();

                //Кол-во
                bool ammountIsParsed = int.TryParse(textBoxItemsAmmount.Text, out int _ammount);
                if (!ammountIsParsed)
                    AddItem.Ammount = null;
                else
                    AddItem.Ammount = _ammount;

                //Цена
                bool priceIsParsed = int.TryParse(textBoxItemsPrice.Text, out int _price);
                if (!priceIsParsed)
                    AddItem.Price = null;
                else
                    AddItem.Price = _price;

                //Дата привоза
                AddItem.ArrivalData = textBoxItemsArrivalData.Text;

                //Период хранения (число)
                bool lifeTimeIsParsed = int.TryParse(textBoxItemsPeriodOfStorage.Text, out int _lifeTime);
                if (!lifeTimeIsParsed)
                    AddItem.PeriodOfStorage = null;
                else
                    AddItem.PeriodOfStorage = _lifeTime;

                //Категория
                if (ComboBoxItemsCategory.Text != "")
                {
                    Categories category = db.Categories.Where(x => x.Name == ComboBoxItemsCategory.Text).FirstOrDefault();
                    if (category != null)
                        AddItem.FK_Category = category.Name;
                }

                else
                    AddItem.FK_Category = null;

                //Обновление срока службы (дата списания)
                if (textBoxItemsArrivalData.Text != "")
                {
                    if (textBoxItemsPeriodOfStorage.Text != "")
                    {
                        DateTime buyDateFromTextBox = Convert.ToDateTime(textBoxItemsArrivalData.Text);
                        AddItem.OffData = buyDateFromTextBox.AddMonths(_lifeTime).ToString("dd.MM.yyyy");
                    }
                }

                else
                {
                    AddItem.OffData = null;

                }

                //Ответственный сотрудник
                if (ComboBoxItemsResponsibleEmployee.Text != "")
                {
                    Employees employee = db.Employees.Where(x => x.Name == ComboBoxItemsResponsibleEmployee.Text).FirstOrDefault();
                    if (employee != null)
                        AddItem.FK_ResponsibleEmployee = employee.Name;
                }

                else
                    AddItem.FK_ResponsibleEmployee = null;

                 
                db.Entry(AddItem).State = EntityState.Added;
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
                if (Validation.TextChanged(textBoxStaffItemsName) && Validation.TextChanged(textBoxItemsAmmount)
                    && Validation.TextChanged(textBoxItemsPrice) && Validation.TextChanged(textBoxItemsArrivalData)
                    && Validation.TextChanged(textBoxItemsPeriodOfStorage))
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

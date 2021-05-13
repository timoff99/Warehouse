using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace Warehouse.Views.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminEmployeesRedactorPage.xaml
    /// </summary>
    public partial class AdminEmployeesRedactorPage : UserControl
    {
        DatabaseEntities db = new DatabaseEntities();
        public AdminEmployeesRedactorPage()
        {
            InitializeComponent();
            FillDataGrid();
        }
        public void FillDataGrid()
        {
            using ( db = new DatabaseEntities())
            {
                var allItems = db.Employees;
                BindingList<Employees> Employees = new BindingList<Employees>();
                foreach (Employees s in allItems)
                {

                    Employees.Add(s);
                }
                EmployeesDataGrid.ItemsSource = Employees;
            }
        }

        private void TextBoxItemsSearchGotFocus_Event(object sender, RoutedEventArgs e)
        {
            textBoxItemsSearch.Text = "";
        }

        private void TextBoxChangedSearchItems_Event(object sender, KeyEventArgs e)
        {
            using ( db = new DatabaseEntities()) // в фильтре многовато условий подчистить 
            {
                var filter = db.Employees.Where(x => x.Name.Contains(textBoxItemsSearch.Text) ||
                                                x.Phone.ToString().Contains(textBoxItemsSearch.Text) ||
                                                x.Email.ToString().Contains(textBoxItemsSearch.Text)
                                                );
                BindingList<Employees> Employees = new BindingList<Employees>();
                foreach (var item in filter)
                {
                    Employees.Add(item);

                }
                EmployeesDataGrid.ItemsSource = Employees;
            }
        }

        private void ButtonResetSearchItems_Click(object sender, RoutedEventArgs e)
        {
            textBoxItemsSearch.Text = "Поиск...";
            FillDataGrid();
        }

        private void GoodsInfoValidation_TextChangedEvent(object sender, KeyEventArgs e)
        {
            if (EmployeesDataGrid.SelectedCells.Count == 3)
            {
                TextBox tb = sender as TextBox;
                Validation.TextChanged(tb);
                if (Validation.TextChanged(textBoxEmployeeName) && Validation.TextChanged(textBoxEmployeePhone)
                    && Validation.TextChanged(textBoxEmployeeEmail))
                    ButtonUpdateGoodsInfo.IsEnabled = true; //отключение кнопки обновить
                else
                    ButtonUpdateGoodsInfo.IsEnabled = false;
            }
        }

        private void ButtonUpdateGoodsInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateAndSaveChanges("Employees");
        }
        public void UpdateAndSaveChanges(string WhatSave)
        {
            try
            {
                using ( db = new DatabaseEntities())
                {
                    //Логика обновления СОТРУДНИКА
                    if (WhatSave == "Employees")
                    {
                        if (EmployeesDataGrid.SelectedCells.Count == 3)
                        {
                            Employees employee = EmployeesDataGrid.SelectedItem as Employees;
                            var oldName = employee.Name;
                            var updatedStaffItem = db.StaffItems.Where(x => x.FK_ResponsibleEmployee == oldName);

                            //поиск обновляемого элемента в контексте по ID (Primary key)
                            employee = db.Employees.Where(x => x.Id == employee.Id).FirstOrDefault();
                            employee.Name = textBoxEmployeeName.Text.ToString();
                            employee.Phone = textBoxEmployeePhone.Text.ToString();

                            foreach (var item in updatedStaffItem)
                            {
                                item.FK_ResponsibleEmployee = textBoxEmployeeName.Text;
                            }
                            db.Entry(employee).State = EntityState.Modified;
                           
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

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeesWindow addEmployeesWindow = new AddEmployeesWindow();
            addEmployeesWindow.ShowDialog();
            FillDataGrid();
        }
        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            using (db = new DatabaseEntities())
            {
                Employees selectedEmployee = EmployeesDataGrid.SelectedItem as Employees;

                var EmployeeItem = db.Employees.Where(x => x.Id == selectedEmployee.Id).FirstOrDefault();

                StaffItems deleteItem = new StaffItems();

                var staffDeleteItem = db.StaffItems.Where(x=> x.FK_ResponsibleEmployee == selectedEmployee.Name);

                foreach (var item in staffDeleteItem)
                {
                    if (item.FK_ResponsibleEmployee == EmployeeItem.Name)
                        item.FK_ResponsibleEmployee = null;
                }
                db.Employees.Remove(EmployeeItem);
                db.Entry(EmployeeItem).State = EntityState.Deleted;
                db.SaveChanges();

                FillDataGrid();
            }
        }
    }
}
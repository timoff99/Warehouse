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

namespace Warehouse.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminAllStaffRedactorPage.xaml
    /// </summary>
    public partial class AdminAllStaffRedactorPage : UserControl
    {
        DatabaseEntities db = new DatabaseEntities();
        public AdminAllStaffRedactorPage()
        {
            InitializeComponent();
            FillDataGrid();
        }
        public void FillDataGrid()
        {
            using (db = new DatabaseEntities())
            {
                var allItems = db.StaffItems;
                BindingList<StaffItems> staffItems = new BindingList<StaffItems>();
                foreach (StaffItems s in allItems)
                {

                    staffItems.Add(s);
                }
                MainDataGrid.ItemsSource = staffItems;
            }
        }

        public void FillComboBox(ComboBox cb)
        {
            using (db = new DatabaseEntities())
            {
                //«Ответственное лицо»
                if (cb == comboBoxItrmsResponsibleHuman)
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
                if (cb == comboBoxItemsCategories )
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

        private void TextBoxItemsSearchGotFocus_Event(object sender, RoutedEventArgs e)
        {
            textBoxItemsSearch.Text = "";
        }

        private void TextBoxChangedSearchItems_Event(object sender, KeyEventArgs e)
        {
            using(db = new DatabaseEntities()) // в фильтре многовато условий подчистить 
            {
                var filter = db.StaffItems.Where(x => x.ItemName.Contains(textBoxItemsSearch.Text) ||
                                                x.Ammount.ToString().Contains(textBoxItemsSearch.Text) ||
                                                x.Price.ToString().Contains(textBoxItemsSearch.Text) ||
                                                x.ArrivalData.Contains(textBoxItemsSearch.Text) ||
                                                x.PeriodOfStorage.ToString().Contains(textBoxItemsSearch.Text) ||
                                                x.OffData.Contains(textBoxItemsSearch.Text) ||
                                                x.FK_ResponsibleEmployee.ToString().Contains(textBoxItemsSearch.Text) ||
                                                x.FK_Category.ToString().Contains(textBoxItemsSearch.Text));
                BindingList<StaffItems> staffItems = new BindingList<StaffItems>();
                foreach (var item in filter)
                {
                    staffItems.Add(item);

                }
                MainDataGrid.ItemsSource = staffItems;
            }
        }

        private void ButtonResetSearchItems_Click(object sender, RoutedEventArgs e)
        {
            textBoxItemsSearch.Text = "Поиск...";
            FillDataGrid();
        }

        private void GoodsInfoValidation_TextChangedEvent(object sender, KeyEventArgs e)
        {
            if (MainDataGrid.SelectedCells.Count == 9)
            {
                TextBox tb = sender as TextBox;
                Validation.TextChanged(tb);
                if (Validation.TextChanged(textBoxStaffItemsName) && Validation.TextChanged(textBoxItemsAmmount)
                    && Validation.TextChanged(textBoxItemsPrice) && Validation.TextChanged(textBoxItemsArrivalData)
                    && Validation.TextChanged(textBoxItemsPeriodOfStorage))
                    ButtonUpdateGoodsInfo.IsEnabled = true; //отключение кнопки обновить
                else
                    ButtonUpdateGoodsInfo.IsEnabled = false;
            }
        }

        private void ButtonUpdateGoodsInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateAndSaveChanges("GoodsInfo");
        }
        public void UpdateAndSaveChanges(string WhatSave)
        {
            try
            {
                using (db = new DatabaseEntities())
                {

                    //Логика обновления ВСЁ ИМУЩЕСТВО
                    if (WhatSave == "GoodsInfo")
                    {
                        if (MainDataGrid.SelectedCells.Count == 9)
                        {
                            //выделеная строка
                            StaffItems rowView = MainDataGrid.SelectedItem as StaffItems;

                            StaffItems updatedStaffItem = new StaffItems();
                            //поиск обновляемого элемента в контексте по ID (Primary key)
                            updatedStaffItem = db.StaffItems.Where(x => x.Id == rowView.Id).FirstOrDefault();

                            //Название
                            updatedStaffItem.ItemName = textBoxStaffItemsName.Text.ToString();

                            //Кол-во
                            bool ammountIsParsed = int.TryParse(textBoxItemsAmmount.Text, out int _ammount);
                            if (!ammountIsParsed)
                                updatedStaffItem.Ammount = null;
                            else
                                updatedStaffItem.Ammount = _ammount;

                            //Цена
                            bool priceIsParsed = int.TryParse(textBoxItemsPrice.Text, out int _price);
                            if (!priceIsParsed)
                                updatedStaffItem.Price = null;
                            else
                                updatedStaffItem.Price = _price;

                            //Дата покупки
                            updatedStaffItem.ArrivalData = textBoxItemsArrivalData.Text;

                            //Срок службы (число)
                            bool lifeTimeIsParsed = int.TryParse(textBoxItemsPeriodOfStorage.Text, out int _lifeTime);
                            if (!lifeTimeIsParsed)
                                updatedStaffItem.PeriodOfStorage = null;
                            else
                                updatedStaffItem.PeriodOfStorage = _lifeTime;

                            //Категория
                            if (comboBoxItemsCategories.Text != "")
                            {
                                Categories category = db.Categories.Where(x => x.Name == comboBoxItemsCategories.Text).FirstOrDefault();
                                if (category != null)
                                    updatedStaffItem.FK_Category = category.Name;
                            }

                            else
                                updatedStaffItem.FK_Category = null;

                            //Обновление срока службы (дата списания)
                            if (textBoxItemsArrivalData.Text != "")
                            {
                                if (textBoxItemsPeriodOfStorage.Text != "")
                                {
                                    DateTime buyDateFromTextBox = Convert.ToDateTime(textBoxItemsArrivalData.Text);
                                    updatedStaffItem.OffData = buyDateFromTextBox.AddMonths(_lifeTime).ToString("dd.MM.yyyy");
                                }
                            }

                            else
                            {
                                updatedStaffItem.OffData = null;

                            }

                            //Ответственный сотрудник
                            if (comboBoxItrmsResponsibleHuman.Text != "")
                            {
                                Employees employee = db.Employees.Where(x => x.Name == comboBoxItrmsResponsibleHuman.Text).FirstOrDefault();
                                if (employee != null)
                                    updatedStaffItem.FK_ResponsibleEmployee = employee.Name;
                            }

                            else
                                updatedStaffItem.FK_ResponsibleEmployee = null;


                            db.Entry(updatedStaffItem).State = EntityState.Modified;
                            db.SaveChanges();

                            FillDataGrid();
                        }
                    }

                    ////Логика обновления СОТРУДНИКА
                    //if (WhatSave == "Employees")
                    //{
                    //    if (EmployeesDataGrid.SelectedCells.Count == 4)
                    //    {
                    //        DataRowView rowView = EmployeesDataGrid.SelectedItem as DataRowView;
                    //        //id выделенной строки
                    //        int selectedItem_ID = Convert.ToInt32(rowView.Row[0]);

                    //        Employees updatedEmployee = new Employees();

                    //        //поиск обновляемого элемента в контексте по ID (Primary key)
                    //        updatedEmployee = db.Employees.Where(x => x.ID_Employee == selectedItem_ID).FirstOrDefault();
                    //        updatedEmployee.Name = textBoxEmployeeName.Text.ToString();
                    //        updatedEmployee.Phone = textBoxEmployeePhone.Text.ToString();
                    //        updatedEmployee.Email = textBoxEmployeeEmail.Text.ToString();

                    //        db.Entry(updatedEmployee).State = EntityState.Modified;
                    //        db.SaveChanges();

                    //        FillDataGrid("EmployeesTable");

                    //    }
                    //}

                    ////Логика обновления КАТЕГОРИИ 
                    //if (WhatSave == "Categories")
                    //{
                    //    if (CategoryesDataGrid.SelectedCells.Count == 3)
                    //    {
                    //        DataRowView rowView = CategoryesDataGrid.SelectedItem as DataRowView;
                    //        //id выделенной строки
                    //        int selectedItem_ID = Convert.ToInt32(rowView.Row[0]);

                    //        Categories category = new Categories();

                    //        //поиск обновляемого элемента в контексте по ID (Primary key)
                    //        category = db.Categories.Where(x => x.ID == selectedItem_ID).FirstOrDefault();
                    //        category.Name = textBoxCategoryName.Text.ToString();
                    //        category.Description = textBoxCategoryDescription.Text.ToString();


                    //        db.Entry(category).State = EntityState.Modified;
                    //        db.SaveChanges();

                    //        FillDataGrid("CategoriesTable");

                    //    }
                    //}

                }
            }
            catch (Exception exteption)
            {
                MessageBox.Show($"Информация об ошибке: {exteption.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox(comboBoxItrmsResponsibleHuman);
            FillComboBox(comboBoxItemsCategories);
            FillDataGrid();
        }

        private void OpenAddItemButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemWindow addItemWindow = new AddItemWindow();
            addItemWindow.ShowDialog();
            FillDataGrid();
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            using(db = new DatabaseEntities())
            {
                StaffItems selectedItem = MainDataGrid.SelectedItem as StaffItems;

                var item = db.StaffItems.Where(x => x.Id == selectedItem.Id).FirstOrDefault();
                db.StaffItems.Remove(item);
                db.Entry(item).State = EntityState.Deleted;
                db.SaveChanges();

                FillDataGrid();
            }
        }
    }
}

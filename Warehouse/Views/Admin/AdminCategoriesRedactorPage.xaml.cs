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
    public partial class AdminCategoriesRedactorPage : UserControl
    {
        DatabaseEntities db = new DatabaseEntities();
        public AdminCategoriesRedactorPage()
        {
            InitializeComponent();
            FillDataGrid();
        }
        public void FillDataGrid()
        {
            using (db = new DatabaseEntities())
            {
                var allItems = db.Categories;
                BindingList<Categories> categories = new BindingList<Categories>();
                foreach (Categories s in allItems)
                {

                    categories.Add(s);
                }
                CategoriesDataGrid.ItemsSource = categories;
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
                var filter = db.Categories.Where(x => x.Name.Contains(textBoxItemsSearch.Text) ||
                                                x.Description.ToString().Contains(textBoxItemsSearch.Text)
                                                );
                BindingList<Categories> categories = new BindingList<Categories>();
                foreach (var item in filter)
                {
                    categories.Add(item);

                }
                CategoriesDataGrid.ItemsSource = categories;
            }
        }

        private void ButtonResetSearchItems_Click(object sender, RoutedEventArgs e)
        {
            textBoxItemsSearch.Text = "Поиск...";
            FillDataGrid();
        }

        private void GoodsInfoValidation_TextChangedEvent(object sender, KeyEventArgs e)
        {
            if (CategoriesDataGrid.SelectedCells.Count == 3)
            {
                TextBox tb = sender as TextBox;
                Validation.TextChanged(tb);
                if (Validation.TextChanged(textBoxCategoryName) && Validation.TextChanged(textBoxCategoryDescription))
                    ButtonUpdateGoodsInfo.IsEnabled = true; //отключение кнопки обновить
                else
                    ButtonUpdateGoodsInfo.IsEnabled = false;
            }
        }

        private void ButtonUpdateGoodsInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateAndSaveChanges("Categories");
        }
        public void UpdateAndSaveChanges(string WhatSave)
        {
            try
            {
                using (db = new DatabaseEntities())
                {
                    //Логика обновления КАТЕГОРИИ 
                    if (WhatSave == "Categories")
                    {
                        if (CategoriesDataGrid.SelectedCells.Count == 2)
                        {
                            StaffItems updatedStaffItem = new StaffItems();

                            Categories category = CategoriesDataGrid.SelectedItem as Categories;

                            //поиск обновляемого элемента в контексте по ID (Primary key)
                            category = db.Categories.Where(x => x.Id == category.Id).FirstOrDefault();
                            category.Name = textBoxCategoryName.Text.ToString();
                            category.Description = textBoxCategoryDescription.Text.ToString();


                            db.Entry(category).State = EntityState.Modified;
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
            AddCategoriesWindow addCategoriesWindow = new AddCategoriesWindow();
            addCategoriesWindow.ShowDialog();
            FillDataGrid();
        }
        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            using (db = new DatabaseEntities())
            {
                Categories selectedItem = CategoriesDataGrid.SelectedItem as Categories;

                var item = db.Categories.Where(x => x.Id == selectedItem.Id).FirstOrDefault();
                db.Categories.Remove(item);
                db.Entry(item).State = EntityState.Deleted;
                db.SaveChanges();

                FillDataGrid();
            }
        }
    }
}
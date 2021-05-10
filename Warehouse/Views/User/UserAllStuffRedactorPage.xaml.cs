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
using QRCoder.Exceptions;
using QRCoder;
using System.IO;
using Microsoft.Win32;

namespace Warehouse.Views.User
{
    /// <summary>
    /// Логика взаимодействия для AdminAllStaffRedactorPage.xaml
    /// </summary>
    public partial class UserAllStuffRedactorPage : UserControl
    {
        DatabaseEntities db = new DatabaseEntities();
        public UserAllStuffRedactorPage()
        {
            InitializeComponent();
            FillDataGrid();
        }
        //Заполнение DataGrid
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

        //Поиск
        private void TextBoxItemsSearchGotFocus_Event(object sender, RoutedEventArgs e)
        {
            textBoxItemsSearch.Text = "";
        }

        private void TextBoxChangedSearchItems_Event(object sender, KeyEventArgs e)
        {
            using (db = new DatabaseEntities()) // в фильтре многовато условий подчистить 
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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
            using (db = new DatabaseEntities())
            {
                StaffItems selectedItem = MainDataGrid.SelectedItem as StaffItems;

                var item = db.StaffItems.Where(x => x.Id == selectedItem.Id).FirstOrDefault();
                db.StaffItems.Remove(item);
                db.Entry(item).State = EntityState.Deleted;
                db.SaveChanges();

                FillDataGrid();
            }
        }

        private void GetQRCode(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedCells.Count == 9)
            {
                try
                {
                    using (db = new DatabaseEntities())
                    {
                        StaffItems rowView = MainDataGrid.SelectedItem as StaffItems;
                        //id выделенной строки


                        StaffItems ItemForWhomGeneratedQR = new StaffItems();
                        ItemForWhomGeneratedQR = db.StaffItems.Where(x => x.Id == rowView.Id).FirstOrDefault();

                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode($"Инвентарный №: {ItemForWhomGeneratedQR.Id} | Наименование: {ItemForWhomGeneratedQR.ItemName} | Дата покупки: {ItemForWhomGeneratedQR.ArrivalData}", QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(150);

                        using (MemoryStream memory = new MemoryStream())
                        {
                            qrCodeImage.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                            memory.Position = 0;
                            SaveFileDialog fldlg = new SaveFileDialog();
                            fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                            fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
                            fldlg.FileName = ItemForWhomGeneratedQR.ItemName;
                            fldlg.ShowDialog();
                            {
                                string imagePath = fldlg.FileName;

                                if (imagePath != "")
                                {
                                    System.Drawing.Image img = System.Drawing.Image.FromStream(memory);
                                    img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                                }
                            }
                        }
                    }
                }
                catch (Exception exteption)
                {
                    MessageBox.Show($"Информация об ошибке: {exteption.Message}", "Произошла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (MainDataGrid.SelectedCells.Count < 10)
                MessageBox.Show($"Вы не выделили ни одного элемента. Нажмите на элемент и попробуйте снова.", "Выделите элемент", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}


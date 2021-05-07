using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Warehouse.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminAllStaffRedactorPage.xaml
    /// </summary>
    public partial class AdminAllStaffRedactorPage : UserControl
    {
        public AdminAllStaffRedactorPage()
        {
            InitializeComponent();
            using(var db = new DatabaseEntities())
            {
                var a = db.StaffItems;
                BindingList<StaffItems> students = new BindingList<StaffItems>();
                foreach (StaffItems s in a)
                {
                    
                    students.Add(s);
                }
                MainDataGrid.ItemsSource = students;
            }
           

            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class StaffItems
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> Ammount { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<int> PeriodOfStorage { get; set; }
        public string ArrivalData { get; set; }
        public string OffData { get; set; }
        public string FK_ResponsibleEmployee { get; set; }
        public string FK_Category { get; set; }

        public virtual Categories Categories { get; set; }
        public virtual Employees Employees { get; set; }

    }
}

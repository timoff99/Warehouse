using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class Categories
    {
        public Categories()
        {
            this.StaffItems = new HashSet<StaffItems>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<StaffItems> StaffItems { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class Employees
    {
        public Employees()
        {
            this.StaffItems = new HashSet<StaffItems>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public virtual ICollection<StaffItems> StaffItems { get; set; }
    }

}

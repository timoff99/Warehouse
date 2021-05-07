using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    class DatabaseEntities : DbContext
    {
        public DatabaseEntities() : base("DatabaseEntities")
        { }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<StaffItems> StaffItems { get; set; }
        public virtual DbSet<Users> Users { get; set; }


    }
}

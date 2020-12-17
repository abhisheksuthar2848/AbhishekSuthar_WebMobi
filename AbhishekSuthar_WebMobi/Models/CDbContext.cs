using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AbhishekSuthar_WebMobi.Models
{
    public class CDbContext : DbContext
    {
        public CDbContext() : base("StringDbContext") { }

        public DbSet<Department> departments { get; set; }
        public DbSet<Employee> employees { get; set; }

    }
}
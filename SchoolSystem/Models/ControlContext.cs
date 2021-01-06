using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SchoolSystem.Models
{
    public class ControlContext : DbContext
    {
        public ControlContext() : base("DefaultConnection")
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<SchoolSystem.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<SchoolSystem.Models.Groups> Groups { get; set; }

        public System.Data.Entity.DbSet<SchoolSystem.Models.GroupsDetails> GroupsDetails { get; set; }

        public System.Data.Entity.DbSet<SchoolSystem.Models.Grades> Grades { get; set; }
    }
}
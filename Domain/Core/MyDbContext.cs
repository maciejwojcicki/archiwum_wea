using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Entities;

namespace Domain.Core
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("name=Model1")
        { 
            //Inicjalizacja bazy danych
            Database.SetInitializer<MyDbContext>(new DropCreateDatabaseIfModelChanges<MyDbContext>()); 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureYear(modelBuilder);
        }
        private void ConfigureYear(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Year>()
                .HasMany(h => h.Graduates)
                .WithRequired(w => w.Year)
                .WillCascadeOnDelete(false);
        }


        public DbSet<User> Users { get; set; }
        //public DbSet<Graduate> Graduates { get; set; }
    }

}

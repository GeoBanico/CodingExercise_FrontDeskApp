using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db_FrontDeskApp.Model;

namespace Db_FrontDeskApp.DataConfig
{
    public class DbConfig : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<StoredPackage> StoredPackages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Change Database String Connection
                optionsBuilder.UseSqlServer("Server=Geo-ZephyrusROG;Database=DB_FrontDeskApp;Trusted_Connection=true;TrustServerCertificate=True");
            }
        }

        /*protected override void OnModelCreating(ModelBuilder mB)
        {
            mB.Entity<Customer>()
                .HasOne(a => a.FacilityLink)
                .WithMany(a => a.CustomerList)
                .HasForeignKey(a => a.FacilityId);

        }*/
    }
}

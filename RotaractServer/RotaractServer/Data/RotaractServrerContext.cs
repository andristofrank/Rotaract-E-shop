using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RotaractServer.Models;
using RotaractServer.Models.DAO_Models;

namespace RotaractServer.Data
{
    public class RotaractServrerContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<RotaractServrerContext>(null);
            base.OnModelCreating(modelBuilder);
        }


        public RotaractServrerContext() : base("name=RotaractDatabase")
        {}


        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<DistrictNo> DistrictNos { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Products_audit> ProductsAudits { get; set; }
        public virtual DbSet<DonationProgram> DonationPrograms { get; set; }
        public virtual DbSet<DonationProgram_Audit> DonationProgramAudits { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<ShippingDetails> ShippingDetailses { get; set; }
    }
}
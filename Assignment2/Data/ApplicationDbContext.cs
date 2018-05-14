using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Assignment2.Models;

namespace Assignment2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<OwnerInventory> OwnerInventories { get; set; }
        public DbSet<StoreInventory> StoreInventories { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StockRequest> StockRequests { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Store>().ToTable("Store");
            builder.Entity<Product>().ToTable("Product");
            builder.Entity<OwnerInventory>().ToTable("OwnerInventory");
            builder.Entity<StoreInventory>().ToTable("StoreInventory");
            builder.Entity<StockRequest>().ToTable("StockRequest");
            builder.Entity<ShoppingCart>().ToTable("ShoppingCart");
            builder.Entity<Order>().ToTable("Order");

            builder.Entity<StoreInventory>().HasKey(x => new { x.StoreID, x.ProductID });
            builder.Entity<ShoppingCart>().HasKey(x => new { x.StoreID, x.ProductID });
            builder.Entity<Order>().HasKey(x => new { x.OrderID, x.StoreID, x.ProductID });
        }
    }
}

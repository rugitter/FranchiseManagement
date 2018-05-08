using Assignment2.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Data
{
    public class Assignment2Context : DbContext
    {
        public Assignment2Context (DbContextOptions<Assignment2Context> options) : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OwnerInventory> OwnerInventories { get; set; }
        public DbSet<StoreInventory> StoreInventories { get; set; }
        public DbSet<StockRequest> StockRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>().ToTable("Store");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<OwnerInventory>().ToTable("OwnerInventory");
            modelBuilder.Entity<StoreInventory>().ToTable("StoreInventory");
            modelBuilder.Entity<StockRequest>().ToTable("StockRequest");

            modelBuilder.Entity<StoreInventory>().HasKey(x => new { x.StoreID, x.ProductID });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Models
{
    public class Assignment2Context : DbContext
    {
        public Assignment2Context (DbContextOptions<Assignment2Context> options)
            : base(options)
        {
        }

        public DbSet<Store> Store { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<OwnerInventory> OwnerInventory { get; set; }
        public DbSet<StoreInventory> StoreInventory { get; set; }
        public DbSet<StockRequest> StockRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreInventory>().HasKey(x => new { x.StoreID, x.ProductID });

            modelBuilder.Entity<Store>().ToTable("Store");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<OwnerInventory>().ToTable("OwnerInventory");
            modelBuilder.Entity<StoreInventory>().ToTable("StoreInventory");
            modelBuilder.Entity<StockRequest>().ToTable("StockRequest");        
        }
    }
}

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Assignment2.Models;

namespace Assignment2.Data
{
    public static class SeedData
    {
        public static void Initialize(Assignment2Context context)
        {
            //using (var context = new Assignment2Context(
            //    serviceProvider.GetRequiredService<DbContextOptions<Assignment2Context>>()))
            //{
                // Look for any movies.
                //if (context.Product.Any() || context.Store.Any() || context.StoreInventory.Any() || context.OwnerInventory.Any()
                //    || context.StockRequest.Any())
                if (!context.Stores.Any())
                {
                    context.Stores.AddRange(
                        new Store { //ID = 1,
                            Name = "Melbourne CBD" },
                        new Store { //ID = 2,
                            Name = "North Melbourne" },
                        new Store { Name = "East Melbourne" },
                        new Store { Name = "South Melbourne" },
                        new Store { Name = "West Melbourne" }
                    );
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product { //ID = 1,
                                Name = "Rabbit" },
                        new Product { //ID = 2,
                                Name = "Hat" },
                        new Product { //ID = 3,
                                Name = "Svengali Deck" },
                        new Product { Name = "Floating Hankerchief" },
                        new Product { Name = "Wand" },
                        new Product { Name = "Broomstick" },
                        new Product { Name = "Bang Gun" },
                        new Product { Name = "Cloak of Invisibility" },
                        new Product { Name = "Elder Wand" },
                        new Product { Name = "Resurrection Stone" }
                    );
                    context.SaveChanges();
                }

            if (!context.OwnerInventories.Any())
            {
                context.OwnerInventories.AddRange(
                    new OwnerInventory
                    {   //ProductID = 1, 
                        ProductID = context.Products.Single(p => p.Name == "Rabbit").ProductID,
                        StockLevel = 20 },
                    new OwnerInventory { ProductID = 2, StockLevel = 41 },
                    new OwnerInventory { ProductID = 3, StockLevel = 72 },
                    new OwnerInventory { ProductID = 4, StockLevel = 41 },
                    new OwnerInventory { ProductID = 5, StockLevel = 51 },
                    new OwnerInventory { ProductID = 6, StockLevel = 61 },
                    new OwnerInventory { ProductID = 7, StockLevel = 71 },
                    new OwnerInventory { ProductID = 8, StockLevel = 81 },
                    new OwnerInventory { ProductID = 9, StockLevel = 91 },
                    new OwnerInventory { ProductID = 10, StockLevel = 11 }
                    );
                context.SaveChanges();
            }

            if (!context.StoreInventories.Any())
            {
                context.StoreInventories.AddRange(
                    new StoreInventory { StoreID = 1, ProductID = 1, StockLevel = 16 },
                    new StoreInventory { StoreID = 1, ProductID = 2, StockLevel = 36 },
                    new StoreInventory { StoreID = 1, ProductID = 3, StockLevel = 8 },
                    new StoreInventory { StoreID = 1, ProductID = 4, StockLevel = 5 },
                    new StoreInventory { StoreID = 2, ProductID = 1, StockLevel = 5 },
                    new StoreInventory { StoreID = 2, ProductID = 2, StockLevel = 5 },
                    new StoreInventory { StoreID = 3, ProductID = 5, StockLevel = 5 },
                    new StoreInventory { StoreID = 4, ProductID = 6, StockLevel = 5 }
                );
                context.SaveChanges();
            }

            if (!context.StockRequests.Any())
            {
                context.StockRequests.AddRange(
                    new StockRequest { //ID = 1, 
                        StoreID = 1, ProductID = 1, Quantity = 10 },
                    new StockRequest { //ID = 2,
                        StoreID = 2, ProductID = 2, Quantity = 20 }
                );
                context.SaveChanges();
            }
        }
    }
}

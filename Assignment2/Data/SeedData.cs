﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Assignment2.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { Constants.OwnerRole, Constants.FranchiseeRole, Constants.CustomerRole };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            await EnsureUserHasRole(userManager, "owner@example.com", Constants.OwnerRole);
            await EnsureUserHasRole(userManager, "store@example.com", Constants.FranchiseeRole);
            await EnsureUserHasRole(userManager, "customer@example.com", Constants.CustomerRole);
            await EnsureUserHasRole(userManager, "s3181520@student.rmit.edu.au", Constants.OwnerRole);
            await EnsureUserHasRole(userManager, "s3679535@student.rmit.edu.au", Constants.OwnerRole);
            await EnsureUserHasRole(userManager, "holder1@example.com", Constants.FranchiseeRole);
            await EnsureUserHasRole(userManager, "holder2@example.com", Constants.FranchiseeRole);
            //await EnsureUserHasRole(userManager, "csuchq@gmail.com", Constants.FranchiseeRole);
            //await EnsureUserHasRole(userManager, "csuchq@hotmail.com", Constants.CustomerRole);

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Name = "Rabbit" },
                    new Product { Name = "Hat" },
                    new Product { Name = "Svengali Deck" },
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
                    {
                        //ProductID = context.Product.Single(p => p.Name == "Rabbit").ID,
                        ProductID = 1,
                        StockLevel = 50
                    },
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

            if (!context.Stores.Any())
            {
                context.Stores.AddRange(
                    new Store { Name = "Melbourne CBD" },
                    new Store { Name = "North Melbourne" },
                    new Store { Name = "East Melbourne" },
                    new Store { Name = "South Melbourne" },
                    new Store { Name = "West Melbourne" }
                );
                context.SaveChanges();
            }

            if (!context.StockRequests.Any())
            {
                context.StockRequests.AddRange(
                    new StockRequest { StoreID = 1, ProductID = 1, Quantity = 10 },
                    new StockRequest { StoreID = 2, ProductID = 2, Quantity = 20 },
                    new StockRequest { StoreID = 3, ProductID = 3, Quantity = 30 }
                );
                context.SaveChanges();
            }

            if (!context.StoreInventories.Any())
            {
                context.StoreInventories.AddRange(
                    new StoreInventory { StoreID = 1, ProductID = 1, StockLevel = 16 },
                    new StoreInventory { StoreID = 1, ProductID = 2, StockLevel = 36 },
                    new StoreInventory { StoreID = 1, ProductID = 3, StockLevel = 81 },
                    new StoreInventory { StoreID = 1, ProductID = 4, StockLevel = 35 },
                    new StoreInventory { StoreID = 1, ProductID = 5, StockLevel = 15 },
                    new StoreInventory { StoreID = 1, ProductID = 6, StockLevel = 55 },
                    new StoreInventory { StoreID = 1, ProductID = 7, StockLevel = 35 },
                    new StoreInventory { StoreID = 1, ProductID = 8, StockLevel = 75 },
                    new StoreInventory { StoreID = 2, ProductID = 1, StockLevel = 21 },
                    new StoreInventory { StoreID = 2, ProductID = 2, StockLevel = 22 },
                    new StoreInventory { StoreID = 2, ProductID = 3, StockLevel = 22 },
                    new StoreInventory { StoreID = 2, ProductID = 4, StockLevel = 22 },
                    new StoreInventory { StoreID = 3, ProductID = 5, StockLevel = 35 },
                    new StoreInventory { StoreID = 4, ProductID = 6, StockLevel = 46 }
                );
                context.SaveChanges();
            }

            //if (!context.StockRequests.Any())
            //{
            //    context.StockRequests.AddRange(
            //        new StockRequest { StoreID = 1, ProductID = 1, Quantity = 10 },
            //        new StockRequest { StoreID = 2, ProductID = 2, Quantity = 20 },
            //        new StockRequest { StoreID = 3, ProductID = 3, Quantity = 30 }
            //    );
            //    context.SaveChanges();
            //}

            //if (!context.ShoppingCarts.Any())
            //{
            //    context.ShoppingCarts.AddRange(
            //        new ShoppingCart { StoreID = 1, ProductID = 1, Quantity = 1 },
            //        new ShoppingCart { StoreID = 1, ProductID = 2, Quantity = 2 },
            //        new ShoppingCart { StoreID = 2, ProductID = 5, Quantity = 5 }
            //    );
            //    context.SaveChanges();
            //}

            //if (!context.Orders.Any())
            //{
            //    context.Orders.AddRange(
            //        new Order { StoreID = 1, ProductID = 1, Quantity = 1 },
            //        new Order { StoreID = 1, ProductID = 2, Quantity = 2 },
            //        new Order { StoreID = 2, ProductID = 5, Quantity = 5 }
            //    );
            //    context.SaveChanges();
            //}
        }

        private static async Task EnsureUserHasRole(
            UserManager<ApplicationUser> userManager, string userName, string role)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user != null && !await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
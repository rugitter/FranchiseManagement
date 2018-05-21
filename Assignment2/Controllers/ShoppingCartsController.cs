using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Microsoft.AspNetCore.Authorization;

namespace Assignment2.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShoppingCarts.Include(s => s.Product).Include(s => s.Store);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id, int? productID )
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(m => m.StoreID == id && m.ProductID == productID);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", shoppingCart.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name", shoppingCart.StoreID);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreID,ProductID,Quantity")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.StoreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.StoreID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", shoppingCart.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name", shoppingCart.StoreID);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(int? id, int? productID)
        {
            if (id == null || productID == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.Product)
                .Include(s => s.Store)
                .SingleOrDefaultAsync(m => m.StoreID == id && m.ProductID == productID);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int productID)
        {
            var shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(m => m.StoreID == id && m.ProductID == productID);
            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ShoppingCarts/CheckOut
        public async Task<ActionResult> CheckOut()
        {
            try
            {
                if (_context.ShoppingCarts.Any())
                {
                    var cart = _context.ShoppingCarts.Include(c => c.Product).Include(c => c.Store);

                    int oID = 1;
                    if (_context.Orders.Any())
                    {
                        oID = _context.Orders.Max(o => o.OrderID);
                        oID++;
                    }

                    Order order = new Order { OrderID = oID, OrderDate = DateTime.Now.ToLocalTime()};
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    foreach (var c in cart)
                    {
                        OrderItem orderItem = new OrderItem
                            { OrderID = oID, StoreID = c.StoreID, ProductID = c.ProductID, Quantity = c.Quantity };
                        _context.OrderItems.Add(orderItem);
                    }
                    await _context.SaveChangesAsync();

                    var shoppingCartToRemove = _context.ShoppingCarts;
                    _context.RemoveRange(shoppingCartToRemove);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCarts.Any(e => e.StoreID == id);
        }
    }
}

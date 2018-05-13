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
    [Authorize(Roles = "Owner")]
    public class StockRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StockRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StockRequests.Include(s => s.Product).Include(s => s.Store);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StockRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequests
                .Include(s => s.Product)
                .Include(s => s.Store)
                .SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }

            return View(stockRequest);
        }

        // GET: StockRequests/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name");
            return View();
        }

        // POST: StockRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockRequestID,StoreID,ProductID,Quantity")] StockRequest stockRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", stockRequest.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name", stockRequest.StoreID);
            return View(stockRequest);
        }

        // GET: StockRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequests.SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", stockRequest.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name", stockRequest.StoreID);
            return View(stockRequest);
        }

        // POST: StockRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockRequestID,StoreID,ProductID,Quantity")] StockRequest stockRequest)
        {
            if (id != stockRequest.StockRequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockRequestExists(stockRequest.StockRequestID))
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
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", stockRequest.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name", stockRequest.StoreID);
            return View(stockRequest);
        }

        // GET: StockRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequests
                .Include(s => s.Product)
                .Include(s => s.Store)
                .SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }

            return View(stockRequest);
        }

        // POST: StockRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockRequest = await _context.StockRequests.SingleOrDefaultAsync(m => m.StockRequestID == id);
            _context.StockRequests.Remove(stockRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //StockRequests/Process/5
        public async Task<IActionResult> Process(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequests
                .SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }

            var ownerInventory = await _context.OwnerInventories
                .SingleOrDefaultAsync(o => o.ProductID == stockRequest.ProductID);
            // Generally impossible unless some extremely serious error
            if (ownerInventory == null)      
            {
                return NotFound();
            }

            if (stockRequest.Quantity > ownerInventory.StockLevel)
            {
                return NotFound();     // View(stockRequest);      // Should display Not sufficient reminder
            }
            else
                ownerInventory.StockLevel = ownerInventory.StockLevel - stockRequest.Quantity;

            var storeInventory = await _context.StoreInventories
                .SingleOrDefaultAsync(s => s.ProductID == stockRequest.ProductID && s.StoreID == stockRequest.StoreID);

            // If already exists in StoreInventory table, update this entry to increase item amount
            if (storeInventory != null)
            {
                storeInventory.StockLevel = storeInventory.StockLevel + stockRequest.Quantity;
                try
                {
                    _context.Update(storeInventory);
                    _context.Update(ownerInventory);
                    _context.StockRequests.Remove(stockRequest);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerInventoryExists(stockRequest.StockRequestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }  
            }
            // If not existing in StoreInventory Table, Add new entry to this table
            else
            {
                try
                {
                    _context.StoreInventories.Add(new StoreInventory {
                        StoreID = stockRequest.StoreID,
                        ProductID = stockRequest.ProductID,
                        StockLevel = stockRequest.Quantity
                    });
                    _context.Update(ownerInventory);
                    _context.StockRequests.Remove(stockRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerInventoryExists(stockRequest.StockRequestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
            // return View(stockRequest);
        }

        private bool OwnerInventoryExists(int id)
        {
            return _context.OwnerInventories.Any(e => e.ProductID == id);
        }

        // Can delete later when cleaning up code
        private bool StockRequestExists(int id)
        {
            return _context.StockRequests.Any(e => e.StockRequestID == id);
        }

    }
}

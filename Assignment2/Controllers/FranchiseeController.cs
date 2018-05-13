using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2.Data;
using Assignment2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Controllers
{
    public class FranchiseeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FranchiseeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            int? id = user.StoreID;

            // If the user doesn't have a storeID - User is not a FranchiseHolder
            if (id == null)
            {
                return NotFound();      
            }

            var store = await _context.Stores
                .Include(s => s.StoreInventories)
                .ThenInclude(i => i.Product)
                .SingleOrDefaultAsync(m => m.StoreID == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
            // return View(await _context.Stores.ToListAsync());
        }


        // GET: Franchisee/MakeRequest/5
        public async Task<IActionResult> MakeRequest(int? productID)
        {
            var user = await _userManager.GetUserAsync(User);

            // When a user doesn't have a storeID, i.e. User is not a FranchiseHolder
            if (user.StoreID == null)
            {
                return NotFound();
            }
            ViewData["StoreID"] = user.StoreID;
            // new SelectList(_context.Stores, "StoreID", "Name", stockRequest.StoreID);

            if (productID == null)
            {
                return NotFound();
            }
            var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductID == productID);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = productID;
            //new SelectList(_context.Products, "ProductID", "Name", stockRequest.ProductID);
            
            return View();
        }

        // POST: StockRequests/MakeRequest/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeRequest([Bind("StoreID,ProductID,Quantity")] StockRequest stockRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.StockRequests.Add(stockRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["StoreID"] = stockRequest.StoreID;
            ViewData["ProductID"] = stockRequest.ProductID;

            return View();
        }

        // GET: Franchisee/AddItem/
        public async Task<IActionResult> AddItem()
        {
            var user = await _userManager.GetUserAsync(User);
            int? sID = user.StoreID;

            // When a user doesn't have a storeID, i.e. User is not a FranchiseHolder
            if (sID == null)
            {
                return NotFound();
            }
            ViewData["StoreID"] = sID;
            // new SelectList(_context.Stores, "StoreID", "Name", stockRequest.StoreID);

            // var store = _context.Stores.SingleOrDefaultAsync(s => s.StoreID == sID);

            IEnumerable<Product> currentProducts = _context.StoreInventories.Include(i => i.Product)
                                                   .Where(i => i.StoreID == sID).Select(i => i.Product);

            //var store = await _context.Stores
            //.Include(s => s.StoreInventories)
            //.ThenInclude(i => i.Product)
            //.SingleOrDefaultAsync(m => m.StoreID == id);

            var newProducts = _context.Products.Except(currentProducts);
            //p => p.ProductID != currentProducts.ProductID);

            //_context.Stores.).SingleOrDefaultAsync(p => p.ProductID == id);
            if (newProducts.Any())
            {
                return NotFound();          // Should Display No New Item available to add, Return
            }

            ViewData["ProductID"] = new SelectList(newProducts, "ProductID", "Name");
            // ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name");

            return View();
        }

        // POST: Franchisee/AddItem/
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem([Bind("StoreID,ProductID,Quantity")] StockRequest stockRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.StockRequests.Add(stockRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["StoreID"] = stockRequest.StoreID;

            IEnumerable<Product> currentProducts = _context.StoreInventories.Include(i => i.Product)
                .Where(i => i.StoreID == stockRequest.StoreID).Select(i => i.Product);
            var newProducts = _context.Products.Except(currentProducts);
            if (newProducts == null)
            {
                return NotFound();          // Should Display No New Item available to add, Return
            }
            ViewData["ProductID"] = new SelectList(newProducts, "ProductID", "Name", stockRequest.ProductID);

            return View();
        }

    }
}
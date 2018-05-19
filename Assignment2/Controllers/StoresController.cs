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
    [Authorize(Roles = "Customer, Owner")]
    public class StoresController : Controller
    {

        const string SessionKeyCart = "_ShoppingCart";
        private readonly ApplicationDbContext _context;
        private ICollection<ShoppingCart> carts = new List<ShoppingCart>();

        public StoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stores.ToListAsync());
        }



        // GET: Stores/Details/5
        public async Task<IActionResult> Details(int? id, string SearchString)
        {
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
            ViewData["StoreID"] = store.StoreID;
            //var product = store.StoreInventories.Select(s => s.Store);

            var storeInv = _context.StoreInventories
                .Include(si => si.Store)
                .Include(si => si.Product)
                .Where(si => si.StoreID == id);
                // from p in _context.StoreInventories select p;

            if (!String.IsNullOrEmpty(SearchString))
            {
                storeInv = storeInv.Where(si => si.Product.Name.Contains(SearchString));
            }

            return View(storeInv);
        }

        [HttpPost]
        public string Details(string searchString, bool notUsed)
        {
            return "From [HttpPost]Details: filter on " + searchString;
        }

        //// GET: Stores/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Stores/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("StoreID,Name")] Store store)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(store);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(store);
        //}


        // GET: Stores/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var store = await _context.Stores.SingleOrDefaultAsync(m => m.StoreID == id);
        //    if (store == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(store);
        //}

        //// POST: Stores/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("StoreID,Name")] Store store)
        //{
        //    if (id != store.StoreID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(store);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!StoreExists(store.StoreID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(store);
        //}

        // GET: Stores/AddCart/5
        // id - StoreID - default route parameter
        // ProductID is passed as a paramter in URL 
        public async Task<IActionResult> AddCart(int? id, int? productID)
        {
            // Must have both storeID and productID to proceed
            if (id == null || productID == null)
            {
                return NotFound();  
            }

            var store = await _context.Stores.SingleOrDefaultAsync(s => s.StoreID == id);
            if (store == null)      // storeID in the URL exists but not invalid
            {
                return NotFound();
            }

            //If empty return default value, For int type, default value is 0
            var pID = _context.StoreInventories
                .Where(i => i.StoreID == id && i.ProductID == productID)
                .Select(i => i.ProductID)
                .SingleOrDefault();

            if (pID == 0 )      // Product not exists in this store
            {
                return NotFound();
            }
            var product = _context.Products.SingleOrDefault(p => p.ProductID == pID);

            ViewData["StoreName"] = store.Name;
            ViewData["UnitPrice"] = product.UnitPrice;

            ShoppingCart cart =
                new ShoppingCart { StoreID = store.StoreID, ProductID = pID, Quantity = 1 };

            return View(cart);
        }

        // POST: Stores/AddCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCart([Bind("StoreID,ProductID,Quantity")] ShoppingCart cart)
        {
            if (ModelState.IsValid)
            {
                //try
                //{
                //    _context.ShoppingCarts.Add(cart);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!CartEntryExists(cart))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}

                var carts = HttpContext.Session.Get<List<ShoppingCart>>(SessionKeyCart);
                if (carts == null)
                {
                    carts = new List<ShoppingCart>();
                    carts.Add(cart);
                    HttpContext.Session.Set<List<ShoppingCart>>(SessionKeyCart, carts);
                }
                else
                {
                    carts.Add(cart);
                    HttpContext.Session.Set<List<ShoppingCart>>(SessionKeyCart, carts);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(cart);
        }

        // Return true if existing, false if not existing
        private bool CartEntryExists(ShoppingCart cart)
        {
            return _context.ShoppingCarts.Any(c => c.StoreID == cart.StoreID && c.ProductID == cart.ProductID);
        }
    }
}

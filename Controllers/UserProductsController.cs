using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using Microsoft.AspNetCore.Http;
using OnlineStore.Models;
using System.Security.Claims;
using System.Numerics;

namespace OnlineStore.Controllers
{
    public class UserProductsController : Controller
    {
        private readonly OnlineStoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserProductsController(OnlineStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        private Uri GetReferrer()
        {
            var header = _httpContextAccessor.HttpContext.Request.GetTypedHeaders();
            return header.Referer;
        }

        // GET: UserProducts
        public async Task<IActionResult> Index()
        {
            IQueryable<UserProduct> userProducts = _context.UserProduct
                .Include(p => p.SellerProduct).ThenInclude(p => p.Product)
                .Include(p => p.SellerProduct).ThenInclude(p => p.Seller)
                .AsQueryable();


            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserID = userId;


            if (User.IsInRole("User"))
            {
                userProducts = userProducts.Where(p => p.UserId == userId);
                var totalSum = await userProducts.SumAsync(p => p.SellerProduct.Price * p.Quantity);
                ViewBag.TotalSum = totalSum;
            }
                      

            return View(await userProducts.ToListAsync());
        }

        // GET: UserProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProduct
                .Include(u => u.SellerProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProduct == null)
            {
                return NotFound();
            }

            return View(userProduct);
        }

        // GET: UserProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description");
            return View();
        }

        // POST: UserProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SellerProductId,UserId,Quantity")] UserProduct userProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProduct);
                await _context.SaveChangesAsync();
                return Redirect(GetReferrer().ToString());
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description", userProduct.SellerProductId);
            return View(userProduct);
        }

        // GET: UserProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProduct.FindAsync(id);
            if (userProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description", userProduct.SellerProductId);
            return View(userProduct);
        }

        // POST: UserProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,UserId,Quantity")] UserProduct userProduct)
        {
            if (id != userProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProductExists(userProduct.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description", userProduct.SellerProductId);
            return View(userProduct);
        }

        // GET: UserProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProduct
                .Include(u => u.SellerProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProduct == null)
            {
                return NotFound();
            }

            return View(userProduct);
        }

        // POST: UserProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userProduct = await _context.UserProduct.FindAsync(id);
            if (userProduct != null)
            {
                _context.UserProduct.Remove(userProduct);
            }

            await _context.SaveChangesAsync();
            return Redirect(GetReferrer().ToString());
        }

        private bool UserProductExists(int id)
        {
            return _context.UserProduct.Any(e => e.Id == id);
        }
    }
}

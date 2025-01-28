using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class UserProductsController : Controller
    {
        private readonly OnlineStoreContext _context;

        public UserProductsController(OnlineStoreContext context)
        {
            _context = context;
        }

        // GET: UserProducts
        public async Task<IActionResult> Index()
        {
            var onlineStoreContext = _context.UserProduct.Include(u => u.Product);
            return View(await onlineStoreContext.ToListAsync());
        }

        // GET: UserProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProduct
                .Include(u => u.Product)
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
        public async Task<IActionResult> Create([Bind("Id,ProductId,UserId,Quantity")] UserProduct userProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description", userProduct.ProductId);
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description", userProduct.ProductId);
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description", userProduct.ProductId);
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
                .Include(u => u.Product)
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
            return RedirectToAction(nameof(Index));
        }

        private bool UserProductExists(int id)
        {
            return _context.UserProduct.Any(e => e.Id == id);
        }
    }
}

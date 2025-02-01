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
    public class SellerProductsController : Controller
    {
        private readonly OnlineStoreContext _context;

        public SellerProductsController(OnlineStoreContext context)
        {
            _context = context;
        }

        // GET: SellerProducts
        public async Task<IActionResult> Index()
        {
            var onlineStoreContext = _context.SellerProduct.Include(s => s.Product).Include(s => s.Seller);
            return View(await onlineStoreContext.ToListAsync());
        }

        // GET: SellerProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerProduct = await _context.SellerProduct
                .Include(s => s.Product)
                .Include(s => s.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sellerProduct == null)
            {
                return NotFound();
            }

            return View(sellerProduct);
        }

        // GET: SellerProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            ViewData["SellerId"] = new SelectList(_context.Seller, "Id", "Name");
            return View();
        }

        // POST: SellerProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,PublishDate,SellerId,ProductId")] SellerProduct sellerProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sellerProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", sellerProduct.ProductId);
            ViewData["SellerId"] = new SelectList(_context.Seller, "Id", "Name", sellerProduct.SellerId);
            return View(sellerProduct);
        }

        // GET: SellerProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerProduct = await _context.SellerProduct.FindAsync(id);
            if (sellerProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", sellerProduct.ProductId);
            ViewData["SellerId"] = new SelectList(_context.Seller, "Id", "Name", sellerProduct.SellerId);
            return View(sellerProduct);
        }

        // POST: SellerProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,PublishDate,SellerId,ProductId")] SellerProduct sellerProduct)
        {
            if (id != sellerProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sellerProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerProductExists(sellerProduct.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", sellerProduct.ProductId);
            ViewData["SellerId"] = new SelectList(_context.Seller, "Id", "Name", sellerProduct.SellerId);
            return View(sellerProduct);
        }

        // GET: SellerProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerProduct = await _context.SellerProduct
                .Include(s => s.Product)
                .Include(s => s.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sellerProduct == null)
            {
                return NotFound();
            }

            return View(sellerProduct);
        }

        // POST: SellerProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sellerProduct = await _context.SellerProduct.FindAsync(id);
            if (sellerProduct != null)
            {
                _context.SellerProduct.Remove(sellerProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerProductExists(int id)
        {
            return _context.SellerProduct.Any(e => e.Id == id);
        }
    }
}

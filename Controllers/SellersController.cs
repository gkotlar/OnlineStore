using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Data;
using OnlineStore.Interfaces;
using OnlineStore.Models;
using OnlineStore.ViewModels;

namespace OnlineStore.Controllers
{
    public class SellersController : Controller
    {
        private readonly OnlineStoreContext _context;
        readonly IBufferedFileUploadService _bufferedFileUploadService;
        readonly FileDeletionService _fileDeletionService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SellersController(OnlineStoreContext context,
            IBufferedFileUploadService bufferedFileUploadService,
            IWebHostEnvironment webHostEnvironment,
            FileDeletionService fileDeletionService)
        {
            _context = context;
            _bufferedFileUploadService = bufferedFileUploadService;
            _fileDeletionService = fileDeletionService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Sellers
        public async Task<IActionResult> Index()
        {
            var onlineStoreContext = _context.Seller
                .Include(m => m.sellerProducts).ThenInclude(m=>m.Product);
            return View(await onlineStoreContext.ToListAsync());
        }

        // GET: Sellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Seller
                .Include(m => m.sellerProducts).ThenInclude(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // GET: Sellers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sellers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind("PhotoURL,Seller")] SellerViewModel sellerVM)
        {
            if (ModelState.IsValid)
            {
                if (sellerVM.PhotoURL != null)
                {
                    try
                    {
                        var fileName = await _bufferedFileUploadService.UploadFile(sellerVM.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                        if (!fileName.IsNullOrEmpty())
                        {
                            ViewBag.Message = "File Upload Successful";
                            sellerVM.Seller.PhotoURL = fileName;
                        }
                        else
                        {
                            ViewBag.Message = "File Upload Failed";
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log ex
                        ViewBag.Message = "File Upload Failed";
                    }
                }

                _context.Add(sellerVM.Seller);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sellerVM);
        }

        // GET: Sellers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Seller.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }

            SellerViewModel viewModel = new SellerViewModel
            {
                Seller = seller
            };

            return View(viewModel);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Seller, PhotoURL")] SellerViewModel sellerVM)
        {
            if (id != sellerVM.Seller.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (sellerVM.PhotoURL != null)
                    {
                        var fileName = await _bufferedFileUploadService.UploadFile(sellerVM.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                        if (!fileName.IsNullOrEmpty())
                        {
                            ViewBag.Message = "File Upload Successful";
                            _fileDeletionService.DeleteFile(sellerVM.Seller.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                            sellerVM.Seller.PhotoURL = fileName;
                        }
                        else
                        {
                            ViewBag.Message = "File Upload Failed";
                        }
                    }

                    _context.Update(sellerVM.Seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(sellerVM.Seller.Id))
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
            return View(sellerVM);
        }

        // GET: Sellers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Seller
                .Include(m => m.sellerProducts).ThenInclude(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // POST: Sellers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seller = await _context.Seller.FindAsync(id);
            if (seller != null)
            {
                if (seller.PhotoURL != null)
                {
                    _fileDeletionService.DeleteFile(seller.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                }
                _context.Seller.Remove(seller);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
            return _context.Seller.Any(e => e.Id == id);
        }
    }
}

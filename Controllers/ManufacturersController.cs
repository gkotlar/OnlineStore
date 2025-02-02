using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class ManufacturersController : Controller
    {
        private readonly OnlineStoreContext _context;
        readonly IBufferedFileUploadService _bufferedFileUploadService;
        readonly FileDeletionService _fileDeletionService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManufacturersController(OnlineStoreContext context, 
            IBufferedFileUploadService bufferedFileUploadService,
            IWebHostEnvironment webHostEnvironment,
            FileDeletionService fileDeletionService)
        {
            _context = context;
            _bufferedFileUploadService = bufferedFileUploadService;
            _fileDeletionService = fileDeletionService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Manufacturers
        public async Task<IActionResult> Index()
        {
            var onlineStoreContext = _context.Manufacturer.Include(m=>m.Products);
            return View(await onlineStoreContext.ToListAsync());
        }

        // GET: Manufacturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturer
                .Include(m=>m.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }

        // GET: Manufacturers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PhotoURL,Manufacturer")] ManufacturerViewModel manufacturerVM)
        {
            if (ModelState.IsValid)
            {
                if (manufacturerVM.PhotoURL != null)
                {
                    try
                    {
                        var fileName = await _bufferedFileUploadService.UploadFile(manufacturerVM.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                        if (!fileName.IsNullOrEmpty())
                        {
                            ViewBag.Message = "File Upload Successful";
                            manufacturerVM.Manufacturer.PhotoURL = fileName;
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

                _context.Add(manufacturerVM.Manufacturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturerVM);
        }

        // GET: Manufacturers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturer.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            ManufacturerViewModel viewModel = new ManufacturerViewModel
            {
                Manufacturer = manufacturer                
            };

            return View(viewModel);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Manufacturer, PhotoURL")] ManufacturerViewModel manufacturerVM)
        {
            if (id != manufacturerVM.Manufacturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (manufacturerVM.PhotoURL != null)
                    {
                        var fileName = await _bufferedFileUploadService.UploadFile(manufacturerVM.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                        if (!fileName.IsNullOrEmpty())
                        {
                            ViewBag.Message = "File Upload Successful";
                            _fileDeletionService.DeleteFile(manufacturerVM.Manufacturer.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                            manufacturerVM.Manufacturer.PhotoURL = fileName;
                        }
                        else
                        {
                            ViewBag.Message = "File Upload Failed";
                        }
                    }

                    _context.Update(manufacturerVM.Manufacturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufacturerExists(manufacturerVM.Manufacturer.Id))
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
            return View(manufacturerVM);
        }

        // GET: Manufacturers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturer
                .Include(m=>m.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }

        // POST: Manufacturers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manufacturer = await _context.Manufacturer.FindAsync(id);
            if (manufacturer != null)
            {
                if (manufacturer.PhotoURL != null)
                {
                    _fileDeletionService.DeleteFile(manufacturer.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                }
                _context.Manufacturer.Remove(manufacturer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManufacturerExists(int id)
        {
            return _context.Manufacturer.Any(e => e.Id == id);
        }
    }
}

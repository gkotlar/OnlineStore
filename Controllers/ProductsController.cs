﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Interfaces;
using OnlineStore.Models;
using OnlineStore.ViewModels;
using OnlineStore.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Security.Claims;

namespace OnlineStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly OnlineStoreContext _context;
        readonly IBufferedFileUploadService _bufferedFileUploadService;
        readonly FileDeletionService _fileDeletionService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductsController(OnlineStoreContext context, 
            IBufferedFileUploadService bufferedFileUploadService, 
            IWebHostEnvironment webHostEnvironment, 
            FileDeletionService fileDeletionService
            )
        {
            _context = context;
            _bufferedFileUploadService = bufferedFileUploadService;
            _fileDeletionService = fileDeletionService;
            _webHostEnvironment = webHostEnvironment;
        }  
        
        // GET: Products
        public async Task<IActionResult> Index(int? productCategory, int? minPriceSearchInt, int? maxPriceSearchInt,
            string nameSearchString, string manufacturerSearchString, string sellerSearchString, string sortOrder)
        {
            IQueryable<Product> products = _context.Product
                .Include(p => p.productCategories).ThenInclude(p => p.Category)
                .Include(p => p.sellerProducts).ThenInclude(p => p.Seller)
                .Include(p => p.Manufacturer)
                .Include(p=>p.Reviews)
            .AsQueryable();

            var categories = _context.Category.AsEnumerable();
            categories = categories.OrderBy(g => g.Name);

            if (!string.IsNullOrEmpty(nameSearchString))
            {
                products = products.Where(p => p.Name.Contains(nameSearchString));
            }
            if (!string.IsNullOrEmpty(manufacturerSearchString))
            {
                products = products.Where(p => p.Manufacturer.Name.Contains(manufacturerSearchString));
            }
            if (!string.IsNullOrEmpty(sellerSearchString))
            {
                products = products.Where(p => p.sellerProducts.Any(sp => sp.Seller.Name.Contains(sellerSearchString)));
            }
            if (productCategory.HasValue)
            {
                products = products.Where(p => p.productCategories.Any(pc => pc.CategoryId == productCategory));
            }

            var str = products; 

            if (minPriceSearchInt.HasValue)
            {
                products = products.Where(p => p.sellerProducts.Any(sc => sc.Price >= minPriceSearchInt));
            }
            if (maxPriceSearchInt.HasValue)
            {
                products = products.Where(p => p.sellerProducts.Any(sc => sc.Price <= maxPriceSearchInt));
            }

            switch (sortOrder)
            {               
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "manufacturer_asc":
                    products = products.OrderBy(p => p.Manufacturer);
                    break;
                case "manufacturer_desc":
                    products = products.OrderByDescending(p => p.Manufacturer);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.sellerProducts.Min(p => p.Price));
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.sellerProducts.Max(p => p.Price));
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }


            var productSearchVM = new ProductSearchViewModel
            {
                Categories = new SelectList(categories, "Id", "Name"),
                MaxPrice = await str.SelectMany(p => p.sellerProducts).MaxAsync(p => (int?)p.Price) ?? 0,
                MinPrice = await str.SelectMany(p => p.sellerProducts).MinAsync(p => (int?)p.Price) ?? 0,
                Products = await products
                .ToListAsync()
            };                     
            return View(productSearchVM);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Manufacturer)
                .Include(p => p.sellerProducts).ThenInclude(p=>p.Seller)
                .Include(p => p.productCategories).ThenInclude(p=>p.Category)
                .Include(p=>p.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.userId = userId;

            ProductReviewsViewModel viewModel = new ProductReviewsViewModel()
            {
                Product = product,
                Reviews = new Review(),
                UserProduct = new UserProduct()              
            };

            return View(viewModel);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturer, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Description, PhotoURL, UserManualURL, ManufacturerId")] ProductViewModel productVM)
        {

            Product product = new Product()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                UserManualURL = "",
                PhotoURL = "",
                ManufacturerId = productVM.ManufacturerId
            };

            if (ModelState.IsValid)
            {
                if (productVM.PhotoURL != null)
                {
                    try
                    {
                        var fileName = await _bufferedFileUploadService.UploadFile(productVM.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                        if (!fileName.IsNullOrEmpty())
                        {
                            ViewBag.Message = "File Upload Successful";
                            product.PhotoURL = fileName;
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
                if (productVM.UserManualURL != null)
                {
                    try
                    {
                        var fileName = await _bufferedFileUploadService.UploadFile(productVM.UserManualURL, Path.Combine(_webHostEnvironment.WebRootPath, "Files"));
                        if (!fileName.IsNullOrEmpty())
                        {
                            ViewBag.Message = "File Upload Successful";
                            product.UserManualURL = fileName;
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
            
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturer, "Id", "Name", productVM.ManufacturerId);
            return View(productVM);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  await _context.Product.Where(p => p.Id == id)
                .Include(p=>p.productCategories).FirstAsync();
            
            if (product == null)
            {
                return NotFound();
            }

            var categories = _context.Category.AsEnumerable();
            categories = categories.OrderBy(s => s.Name);
            
            ProductCategoriesEditViewModel viewModel = new ProductCategoriesEditViewModel
            {
                Product = product,
                CategoryList = new MultiSelectList(categories, "Id", "Name"),
                SelectedCategories = product.productCategories.Select(sc => sc.CategoryId),
            };

            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturer, "Id", "Name", product.ManufacturerId);
            return View(viewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  [Bind("Product, SelectedCategories, CategoryList, PhotoURL, UserManualURL")] ProductCategoriesEditViewModel viewModel)
        {
            if (id != viewModel.Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (viewModel.PhotoURL != null)
                {
                    try
                    {
                        var fileName = await _bufferedFileUploadService.UploadFile(viewModel.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                        if (!fileName.IsNullOrEmpty())
                        {
                            ViewBag.Message = "File Upload Successful";
                            if (!viewModel.Product.PhotoURL.IsNullOrEmpty())
                            {
                                _fileDeletionService.DeleteFile(viewModel.Product.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                            }
                            viewModel.Product.PhotoURL = fileName;
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
         

                if (viewModel.UserManualURL != null)
                {
                    try
                    {
                        var fileName = await _bufferedFileUploadService.UploadFile(viewModel.UserManualURL, Path.Combine(_webHostEnvironment.WebRootPath, "Files"));
                        if (!fileName.IsNullOrEmpty())
                        {
                            ViewBag.Message = "File Upload Successful";
                            if (!viewModel.Product.UserManualURL.IsNullOrEmpty()) 
                            {
                                _fileDeletionService.DeleteFile(viewModel.Product.UserManualURL, Path.Combine(_webHostEnvironment.WebRootPath, "Files"));
                            }
                            viewModel.Product.UserManualURL = fileName;
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
            
                
                try
                {
                    _context.Update(viewModel.Product);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> newCategoryList = viewModel.SelectedCategories;
                    IEnumerable<int> prevCategoryList = _context.ProductCategory.Where(s => s.ProductId == id).Select(s => s.CategoryId);
                    IQueryable<ProductCategory> toBeRemoved = _context.ProductCategory.Where(s => s.ProductId == id);
                    if (newCategoryList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newCategoryList.Contains(s.CategoryId));
                        foreach (int categoryId in newCategoryList)
                        {
                            if (!prevCategoryList.Any(s => s == categoryId))
                            {
                                _context.ProductCategory.Add(new ProductCategory { CategoryId = categoryId, ProductId = id });
                            }
                        }
                    }
                    _context.ProductCategory.RemoveRange(toBeRemoved);
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(viewModel.Product.Id))
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
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturer, "Id", "Name", viewModel.Product.ManufacturerId);
            return View(viewModel);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Manufacturer)
                .Include(p => p.sellerProducts).ThenInclude(p => p.Seller)
                .Include(p => p.productCategories).ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                if (product.PhotoURL != null)
                {
                    _fileDeletionService.DeleteFile(product.PhotoURL, Path.Combine(_webHostEnvironment.WebRootPath, "Images"));
                }
                if (product.UserManualURL != null)
                {
                    _fileDeletionService.DeleteFile(product.UserManualURL, Path.Combine(_webHostEnvironment.WebRootPath, "Files"));
                }
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}

#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Products
        public async Task<IActionResult> Index(string? sortBy, string? search, int? pageNumber)
        {
            IQueryable<Product> productQuery = _productRepository.GetQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.Trim().ToLower();
                productQuery = productQuery.Where(p => p.Name.ToLower().Contains(loweredSearch));
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "name_asc":
                        productQuery = productQuery.OrderBy(p => p.Name);
                        break;
                    case "name_desc":
                        productQuery = productQuery.OrderByDescending(p => p.Name);
                        break;
                    case "price_asc":
                        productQuery = productQuery.OrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        productQuery = productQuery.OrderByDescending(p => p.Price);
                        break;
                    case "release_asc":
                        productQuery = productQuery.OrderBy(p => p.ReleaseDate);
                        break;
                    case "release_desc":
                        productQuery = productQuery.OrderByDescending(p => p.ReleaseDate);
                        break;
                    case "code_asc":
                        productQuery = productQuery.OrderBy(p => p.ProductCode);
                        break;
                    case "code_desc":
                        productQuery = productQuery.OrderByDescending(p => p.ProductCode);
                        break;
                    default:
                        productQuery = productQuery.OrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                productQuery = productQuery.OrderBy(p => p.Name);
            }

            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSearch = search;

            ViewBag.TotalProducts = await _productRepository.CountAsync(productQuery);
            ViewBag.TotalPrice = await _productRepository.SumPriceAsync(productQuery);

            int pageSize = 5;

            var paginatedProducts = await PaginatedList<Product>.CreateAsync(
                productQuery,
                pageNumber ?? 1,
                pageSize
            );

            return View(paginatedProducts);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductCode,Name,ReleaseDate,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.AddAsync(product);
                await _productRepository.SaveChangesAsync();
                TempData["Message"] = "Successfully added!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Message"] = "Something wrong, please try again!";
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductCode,Name,ReleaseDate,Price")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productRepository.Update(product);
                    await _productRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistsAsync(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Successfully edited!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Message"] = "Something wrong, please try again!";
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                _productRepository.Remove(product);
                await _productRepository.SaveChangesAsync();
            }
            TempData["Message"] = "Successfully deleted!";
            return RedirectToAction(nameof(Index));
        }
    }
}
}

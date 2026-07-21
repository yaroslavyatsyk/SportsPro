#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SportContext _context;

        public ProductsController(SportContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string? sortBy, string? search, int? pageNumber)
        {
            IQueryable<Product> productQuery = _context.Products.AsNoTracking();

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

            ViewBag.TotalProducts = await productQuery.CountAsync();
            ViewBag.TotalPrice = await productQuery.SumAsync(p => p.Price);

            int pageSize = 5;

            var paginatedProducts = await PaginatedList<Product>.CreateAsync(
                productQuery.AsNoTracking(),
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

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
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

                _context.Add(product);
                await _context.SaveChangesAsync();
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

            var product = await _context.Products.FindAsync(id);
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
                    _context.Update(product);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
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
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Successfully deleted!";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

    }
}


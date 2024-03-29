﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

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
        public async Task<IActionResult> Index(string sortBy = "", string search = "")
        {


            var products = await _context.Products.ToListAsync();

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.ToLower().Contains(search)).ToList();

                return View(products);
            }


            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_asc":
                        products = products.OrderBy(p => p.Name).ToList();
                        break;
                    case "name_desc":
                        products = products.OrderByDescending(p => p.Name).ToList();
                        break;
                    case "price_asc":
                        products = products.OrderBy(p => p.Price).ToList();
                        break;
                    case "price_desc":
                        products = products.OrderByDescending(p => p.Price).ToList();
                        break;
                    case "release_asc":
                        products = products.OrderBy(p => p.ReleaseDate).ToList();
                        break;

                    case "release_desc":
                        products = products.OrderByDescending(p => p.ReleaseDate).ToList();
                        break;

                    case "code_asc":
                        products = products.OrderBy(p => p.ProductCode).ToList();
                        break;

                    case "code_desc":
                        products = products.OrderByDescending(p => p.ProductCode).ToList();
                        break;
                }
            

                
            }
            return View(products);
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

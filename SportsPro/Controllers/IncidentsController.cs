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
    public class IncidentsController : Controller
    {
        private readonly SportContext _context;

        public IncidentsController(SportContext context)
        {
            _context = context;
        }

        // GET: Incidents
        public async Task<IActionResult> Index(string filtering = "", string sorting = "")
        {
            var sportContext = await _context.Incidents.Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();

            

            

               switch (filtering)
            {

                case "Unassigned":
                    sportContext = await _context.Incidents.Where(i => i.TechnicianId == null).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                    break;
                case "Opened":

                    sportContext = await _context.Incidents.Where(i => i.DateClosed == null).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                    break;

                    case "Resolved":
                        sportContext = await _context.Incidents.Where(i => i.DateClosed != null).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                        break;

                    default:
                    sportContext = await _context.Incidents.Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                    break;

            }

            if (!string.IsNullOrEmpty(sorting))
            {
                switch (sorting)
                {
                    case "Customer (A-Z)":

                        sportContext = await _context.Incidents.OrderBy(i => i.Customer).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                        break;
                        case "Customer (Z-A)":
                            sportContext = await _context.Incidents.OrderByDescending(i => i.Customer).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                            break;
                    case "Product (A-Z)":

                        sportContext = await _context.Incidents.OrderBy(i => i.Product.Name).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                        break;
                        case "Product (Z-A)":
                            sportContext = await _context.Incidents.OrderByDescending(i => i.Product.Name).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                            break;
                    case "Technician (A-Z)":
                        sportContext = await _context.Incidents.OrderBy(i => i.Technician).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                        break;
                        case "Technician (Z-A)":
                            sportContext = await _context.Incidents.OrderByDescending(i => i.Technician).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                            break;
                    case "DateOpened (A-Z)":
                        sportContext = await _context.Incidents.OrderBy(i => i.DateOpened).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                        break;
                        case "DateOpened (Z-A)":
                            sportContext = await _context.Incidents.OrderByDescending(i => i.DateOpened).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                            break;
                    case "DateClosed (A-Z)":
                        sportContext = await _context.Incidents.OrderBy(i => i.DateClosed).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                        break;
                        case "DateClosed (Z-A)":
                            sportContext = await _context.Incidents.OrderByDescending(i => i.DateClosed).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                            break;
                    case "Title (A-Z)":
                        sportContext = await _context.Incidents.OrderBy(i => i.Title).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                        break;
                        case "Title (Z-A)":
                            sportContext = await _context.Incidents.OrderByDescending(i => i.Title).Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician).ToListAsync();
                            break;

                }

                




                return View(sportContext);
            }




            return View(sportContext);
        }
       
       

        // GET: Incidents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefaultAsync(m => m.IncidentId == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: Incidents/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["TechnicianId"] = new SelectList(_context.Technicianes, "TechnicianId", "FullName");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IncidentId,Title,CustomerId,ProductId,DateOpened,DateClosed,TechnicianId,Description")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", incident.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", incident.ProductId);
            ViewData["TechnicianId"] = new SelectList(_context.Technicianes, "TechnicianId", "FullName", incident.TechnicianId);
            return View(incident);
        }

        // GET: Incidents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", incident.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", incident.ProductId);
            ViewData["TechnicianId"] = new SelectList(_context.Technicianes, "TechnicianId", "FullName", incident.TechnicianId);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IncidentId,Title,CustomerId,ProductId,DateOpened,DateClosed,TechnicianId,Description")] Incident incident)
        {
            if (id != incident.IncidentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(incident.IncidentId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", incident.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", incident.ProductId);
            ViewData["TechnicianId"] = new SelectList(_context.Technicianes, "TechnicianId", "FullName", incident.TechnicianId);
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefaultAsync(m => m.IncidentId == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.IncidentId == id);
        }
    }
}

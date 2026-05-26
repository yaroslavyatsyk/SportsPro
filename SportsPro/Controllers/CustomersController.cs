#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class CustomersController : Controller
    {
        private readonly SportContext _context;

        public CustomersController(SportContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string? sorting, string? fullName, int? pageNumber, string? gender)
        {
            IQueryable<Customer> customerQuery = _context.Customers;

            if (!string.IsNullOrWhiteSpace(sorting))
            {
                switch (sorting)
                {
                    case "FirstNameASC":
                        customerQuery = customerQuery.OrderBy(c => c.FirstName);
                        break;
                    case "FirstNameDESC":
                        customerQuery = customerQuery.OrderByDescending(c => c.FirstName);
                        break;
                    case "LastNameASC":
                        customerQuery = customerQuery.OrderBy(c => c.LastName);
                        break;
                    case "LastNameDESC":
                        customerQuery = customerQuery.OrderByDescending(c => c.LastName);
                        break;
                    case "CityASC":
                        customerQuery = customerQuery.OrderBy(c => c.City);
                        break;
                    case "CityDESC":
                        customerQuery = customerQuery.OrderByDescending(c => c.City);
                        break;
                    case "StateASC":
                        customerQuery = customerQuery.OrderBy(c => c.State);
                        break;
                    case "StateDESC":
                        customerQuery = customerQuery.OrderByDescending(c => c.State);
                        break;
                    default:
                        customerQuery = customerQuery.OrderBy(c => c.FirstName);
                        break;
                }
            }
            else
            {
                customerQuery = customerQuery.OrderBy(c => c.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(fullName))
            {
                fullName = fullName.Trim().ToLower();
                customerQuery = customerQuery.Where(c =>
                    (c.FirstName + " " + c.LastName).ToLower().Contains(fullName));
            }

            if (!string.IsNullOrWhiteSpace(gender))
            {

                customerQuery = customerQuery.Where(c => c.Gender == gender);

            }

            ViewBag.CurrentSorting = sorting;
            ViewBag.CurrentFullName = fullName;
            ViewBag.TotalCustomers = await customerQuery.CountAsync();

            int pageSize = 5;

            var paginatedCustomers = await PaginatedList<Customer>.CreateAsync(
                customerQuery.AsNoTracking(),
                pageNumber ?? 1,
                pageSize
            );

            return View(paginatedCustomers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            List<string> countriesList = getCountries();


            ViewBag.Countries = countriesList;



          
            
           
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,City,State,PostalCode,Country,Email,Phone")] Customer customer)
        {
        
            if (ModelState.IsValid)
            {
                if(String.IsNullOrEmpty(customer.State))
                {
                    customer.State = "Non Applicable";
                }
                _context.Add(customer);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Customer has been added successfully";
                return RedirectToAction(nameof(Index));
            }
         
            return View(customer);
        }


        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            List<string> countriesList = getCountries();
            ViewBag.Countries = countriesList;

            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,City,State,PostalCode,Country,Email,Phone")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Customer has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Customer has been deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        private List<string> getCountries()
        {
            var data = new WebClient().DownloadString("https://restcountries.com/v3.1/all?fields=name,capital,currencies");
            dynamic json = JsonConvert.DeserializeObject(data);
            List<string> listOfCountries = new List<string>();
            foreach (var jsonObject in json)
            {
                if ((string)jsonObject.name.common != "Russia")
                    listOfCountries.Add((string)jsonObject.name.common); 

            }
            listOfCountries.Sort();
            return listOfCountries;
        }
       
      


    }
}

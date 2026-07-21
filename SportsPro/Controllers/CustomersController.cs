#nullable disable
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Repositories;
using SportsPro.Services;

namespace SportsPro.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICountryService _countryService;

        public CustomersController(ICustomerRepository customerRepository, ICountryService countryService)
        {
            _customerRepository = customerRepository;
            _countryService = countryService;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string sorting, string fullName, int? pageNumber, string gender)
        {
            ViewBag.CurrentSorting = sorting;
            ViewBag.CurrentFullName = fullName;
            ViewBag.CurrentGender = gender;
            ViewBag.TotalCustomers = await _customerRepository.GetCustomerCountAsync(fullName, gender);

            int pageSize = 5;
            var paginatedCustomers = await _customerRepository.GetCustomersAsync(
                sorting, fullName, gender, pageNumber ?? 1, pageSize);

            return View(paginatedCustomers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _customerRepository.GetByIdAsync(id.Value);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // GET: Customers/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Countries = await _countryService.GetCountriesAsync();
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CustomerId,FirstName,LastName,City,State,PostalCode,Country,Email,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(customer.State))
                {
                    customer.State = "Non Applicable";
                }

                await _customerRepository.AddAsync(customer);
                TempData["Message"] = "Customer has been added successfully";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Countries = await _countryService.GetCountriesAsync();
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Countries = await _countryService.GetCountriesAsync();

            if (id == null) return NotFound();

            var customer = await _customerRepository.GetByIdAsync(id.Value);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("CustomerId,FirstName,LastName,City,State,PostalCode,Country,Email,Phone")] Customer customer)
        {
            if (id != customer.CustomerId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _customerRepository.UpdateAsync(customer);
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                {
                    if (!await _customerRepository.ExistsAsync(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    throw;
                }

                TempData["Message"] = "Customer has been updated successfully";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Countries = await _countryService.GetCountriesAsync();
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _customerRepository.GetByIdAsync(id.Value);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _customerRepository.DeleteAsync(id);
            TempData["Message"] = "Customer has been deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
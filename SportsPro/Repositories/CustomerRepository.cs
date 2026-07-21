using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System.Linq;

namespace SportsPro.Repositories
    {
        public class CustomerRepository : ICustomerRepository
        {
            private readonly SportContext _context;

            public CustomerRepository(SportContext context)
            {
                _context = context;
            }

            private IQueryable<Customer> BuildQuery(string sorting, string fullName, string gender)
            {
                IQueryable<Customer> query = _context.Customers.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    var loweredFullName = fullName.Trim().ToLower();
                    query = query.Where(c =>
                        (c.FirstName + " " + c.LastName).ToLower().Contains(loweredFullName));
                }

                if (!string.IsNullOrWhiteSpace(gender))
                {
                    query = query.Where(c => c.Gender == gender);
                }

                query = sorting switch
                {
                    "FirstNameASC" => query.OrderBy(c => c.FirstName),
                    "FirstNameDESC" => query.OrderByDescending(c => c.FirstName),
                    "LastNameASC" => query.OrderBy(c => c.LastName),
                    "LastNameDESC" => query.OrderByDescending(c => c.LastName),
                    "CityASC" => query.OrderBy(c => c.City),
                    "CityDESC" => query.OrderByDescending(c => c.City),
                    "StateASC" => query.OrderBy(c => c.State),
                    "StateDESC" => query.OrderByDescending(c => c.State),
                    _ => query.OrderBy(c => c.FirstName),
                };

                return query;
            }

            public async Task<PaginatedList<Customer>> GetCustomersAsync(
                string sorting, string fullName, string gender, int pageNumber, int pageSize)
            {
                var query = BuildQuery(sorting, fullName, gender);
                return await PaginatedList<Customer>.CreateAsync(query, pageNumber, pageSize);
            }

            public async Task<int> GetCustomerCountAsync(string fullName, string gender)
            {
                var query = BuildQuery(null, fullName, gender);
                return await query.CountAsync();
            }

            public async Task<Customer> GetByIdAsync(int id)
            {
                return await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == id);
            }

            public async Task AddAsync(Customer customer)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Customer customer)
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                }
            }

            public async Task<bool> ExistsAsync(int id)
            {
                return await _context.Customers.AnyAsync(c => c.CustomerId == id);
            }
        }
    }
}

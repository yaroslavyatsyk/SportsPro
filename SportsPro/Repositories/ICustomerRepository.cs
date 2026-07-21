
using SportsPro.Models;

namespace SportsPro.Repositories
{
    public interface ICustomerRepository
    {



        Task<PaginatedList<Customer>> GetCustomersAsync(
            string sorting, string fullName, string gender, int pageNumber, int pageSize);

        Task<int> GetCustomerCountAsync(string fullName, string gender);

        Task<Customer> GetByIdAsync(int id);

        Task AddAsync(Customer customer);

        Task UpdateAsync(Customer customer);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);




    }
}

using SportsPro.Models;

namespace SportsPro.Repositories
{
    public interface IProductRepository
    {
        /// <summary>
        /// Returns a queryable, untracked base query so the caller (controller/service)
        /// can compose filtering, sorting and paging on top of it.
        /// </summary>
        IQueryable<Product> GetQueryable();

        Task<Product?> GetByIdAsync(int id);

        Task<int> CountAsync(IQueryable<Product> query);

        Task<decimal> SumPriceAsync(IQueryable<Product> query);

        Task AddAsync(Product product);

        void Update(Product product);

        void Remove(Product product);

        Task<bool> ExistsAsync(int id);

        Task<int> SaveChangesAsync();
    }
}

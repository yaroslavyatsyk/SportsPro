using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SportContext _context;

        public ProductRepository(SportContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetQueryable()
        {
            return _context.Products.AsNoTracking();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<int> CountAsync(IQueryable<Product> query)
        {
            return await query.CountAsync();
        }

        public async Task<decimal> SumPriceAsync(IQueryable<Product> query)
        {
            return (decimal)await query.SumAsync(p => p.Price);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Remove(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

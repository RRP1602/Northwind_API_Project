using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;

namespace NorthwindAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;
      
        public ProductService(NorthwindContext context)
        {
            _context = context;
        }
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductByCategoryIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await _context.Products
                .Include(p => p.OrderDetails)
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .Where(p => p.ProductName == name)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductBySupplierIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.OrderDetails)
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .Where(p => p.SupplierId == id)
                .ToListAsync();
        }

        public bool ProductsExsits(int id)
        {
            return _context.Products.Any(p => p.ProductId == id);
        }

        public async Task RemoveProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

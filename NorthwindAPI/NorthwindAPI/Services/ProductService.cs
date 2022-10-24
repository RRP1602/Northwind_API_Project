using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;

namespace NorthwindAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;

        public ProductService()
        {
            _context = new NorthwindContext();
        }
        public ProductService(NorthwindContext context)
        {
            _context = context;
        }
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public  List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryIdAsync(int id)
        {
            return await _context.Products.Include(x=> x.Category).Where(x => x.CategoryId == id).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            return await _context.Products.Where(x => x.ProductName == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductBySupplierIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool ProductsExsits(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

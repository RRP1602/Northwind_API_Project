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
        public Task AddProductAsync(Product product)
        {
            throw new NotImplementedException();
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

        public Task<Product> GetProductByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductBySupplierIdAsync(int id)
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

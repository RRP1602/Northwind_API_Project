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

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryIdAsync(int id)
        {
            return await _context.Products
                .Include(x=> x.Category)
                .Include(p => p.Supplier)
                .Where(x => x.CategoryId == id).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .Where(p => p.ProductName == name)
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Product>> GetProductBySupplierIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .Where(p => p.SupplierId == id)
                .ToListAsync();
        }

        public bool ProductsExists(int id)
        {
            return _context.Products.Any(p => p.ProductId == id);
        }

        public async Task RemoveProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsInMostPopularCategory()
        {
            var category = await (from p in _context.Products
                                  join od in _context.OrderDetails on p.ProductId equals od.ProductId
                                  group od by od.ProductId into g
                                  orderby g.Count() descending
                                  select new { Id = g.Key, Count = g.Count() }
                            ).Distinct()
                            .FirstOrDefaultAsync();

            return await _context.Products
                .Where(p => p.CategoryId == category.Id)
                .ToListAsync();
        }

        public async Task<Product?> GetBestSellingProduct()
        {
            var products = await (from p in _context.Products
                                  join od in _context.OrderDetails on p.ProductId equals od.ProductId
                                  group od by od.ProductId into g
                                  orderby g.Count() descending
                                  select new { Id = g.Key, Count = g.Count() }
                           ).Distinct()
                           .FirstOrDefaultAsync();

            return await GetProductByIdAsync(products.Id);
        }

        public async Task<IEnumerable<Product>> GetTop3SellingProducts()
        {
            var product = (from p in _context.Products
                           join od in _context.OrderDetails on p.ProductId equals od.ProductId
                           group od by od.ProductId into g
                           orderby g.Count() descending
                           select new { Id = g.Key, Count = g.Count() }
                          ).Distinct()
                          .Take(3)
                          .ToList();

            var bestSelling = await _context.Products
                .Where(p => p.ProductId == product[0].Id || p.ProductId == product[1].Id || p.ProductId == product[2].Id)
                .ToListAsync();

            return bestSelling;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

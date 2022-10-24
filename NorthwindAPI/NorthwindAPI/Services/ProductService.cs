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
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryIdAsync(int id)
        {
            return await _context.Products.Include(x=> x.Category).Where(x => x.CategoryId == id).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
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

        public bool ProductsExists(int id)
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

        public async Task<IEnumerable<Product>> GetProductsInMostPopularCategory()
        {
            var category = await _context.Products
                .GroupBy(p => p.CategoryId)
                .Distinct()
                .OrderByDescending(grp => grp.Count())
                .Select(p => new {Id = p.Key, Count = p.Count()})
                .FirstOrDefaultAsync();

            return await _context.Products
                .Where(p => p.CategoryId == category.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetDiscontinuedProducts()
        {
            return await _context.Products.Where(p => p.Discontinued).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsWithHighestReorderLevel()
        {
            var reorder = await _context.Products
                .Select(p => p)
                .OrderByDescending(p => p.ReorderLevel)
                .ToListAsync();

            return reorder.TakeWhile(p => p.ReorderLevel == reorder.First().ReorderLevel);
        }

        public async Task<IEnumerable<Product>> GetProductWithHighestStock()
        {
            var highest = await _context.Products
                .Select(p => p)
                .OrderByDescending(p => p.UnitsInStock)
                .ToListAsync();

            return highest.TakeWhile(p => p.UnitsInStock == highest.First().UnitsInStock);
        }

        public async Task<IEnumerable<Product>> GetProductsWithLowestStock()
        {
            var highest = await _context.Products
                .Select(p => p)
                .OrderBy(p => p.UnitsInStock)
                .ToListAsync();

            return highest.TakeWhile(p => p.UnitsInStock == highest.First().UnitsInStock);
        }

        public Task<Product> GetBestSellingProduct()
        {
            //var bestSelling = _context.Products
            //    .Include(p => p.OrderDetails)
            //    .GroupBy()
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetTop3SellingProducts()
        {
            throw new NotImplementedException();
        }
    }
}

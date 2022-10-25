using NorthwindAPI.Models;

namespace NorthwindAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(); 
        Task<Product> GetProductByIdAsync(int id);
        Task<Product?> GetProductByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductByCategoryIdAsync(int id);
        Task<IEnumerable<Product>> GetProductBySupplierIdAsync(int id);
        Task AddProductAsync(Product product); 
        Task RemoveProductAsync(Product product);
        Task<IEnumerable<Product>> GetProductsInMostPopularCategory();
        Task<Product?> GetBestSellingProduct();
        Task<IEnumerable<Product>> GetTop3SellingProducts();
        Task<int> SaveChangesAsync();
        bool ProductsExists(int id);
    }
}

using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;

namespace NorthwindAPI.Controllers
{
    public static class Utils
    {
        public static DTOProduct ProductToDto(Product product)
        {
            return new DTOProduct
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                ProductName = product.ProductName,
                SupplierId = product.SupplierId,
                UnitPrice = product.UnitPrice
            };
        }

        public static Product DtoToProduct(DTOProduct dto)
        {
            return new Product
            {
                ProductId = dto.ProductId,
                CategoryId = dto.CategoryId,
                ProductName = dto.ProductName,
                SupplierId = dto.SupplierId,
                UnitPrice = dto.UnitPrice
            };
        }

    }
}

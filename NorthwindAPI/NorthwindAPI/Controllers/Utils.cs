using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;
using System.Diagnostics.CodeAnalysis;

namespace NorthwindAPI.Controllers
{
    [ExcludeFromCodeCoverage]

    public static class Utils
    {
        public static DTOProduct ProductToDto(Product product)
        {
            return new DTOProduct
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                Category = Utils.CategoryToDto(product.Category),
                Supplier = Utils.SupplierToDto(product.Supplier)
            };
        }

        public static Product DtoToProduct(DTOProduct dto)
        {
            return new Product
            {
                ProductId = dto.ProductId,
                Category = DtoToCategory(dto.Category),
                ProductName = dto.ProductName,
                Supplier = DtoToSupplier(dto.Supplier),
                UnitPrice = dto.UnitPrice
            };
        }

        public static DTOSupplier SupplierToDto(Supplier supplier)
        {
            if (supplier == null) return new DTOSupplier
            {

            };
            return new DTOSupplier
            {
                SupplierId = supplier.SupplierId,
                CompanyName = supplier.CompanyName,
                TotalProducts = supplier.Products.Count(),
                Country = supplier.Country
            };
        }

        public static Supplier DtoToSupplier(DTOSupplier dto)
        {
            return new Supplier
            {

                SupplierId = dto.SupplierId,
                CompanyName = dto.CompanyName,
                Country = dto.Country
                
            };
        }

        //public static DTOOrderDetails OrderDetailsToDto(OrderDetail orderDetails)
        //{
        //    if (orderDetails == null) return new DTOOrderDetails
        //    {

        //    };

        //    return new DTOOrderDetails
        //    {
        //        OrderId = orderDetails.OrderId,
        //        ProductId = orderDetails.ProductId,
        //        TotalPrice = (float)orderDetails.UnitPrice,
        //        TotalDiscount = orderDetails.Discount,
        //        TotalProducts = orderDetails.Quantity
        //    };
        //}

        //public static OrderDetail DtoToOrderDetails(DTOOrderDetails dto)
        //{
        //    return new OrderDetail
        //    {

        //        OrderId = dto.OrderId,
        //        ProductId = dto.ProductId,
        //        UnitPrice = (decimal)dto.TotalPrice,
        //        Discount = dto.TotalDiscount,
        //        Quantity = (short)dto.TotalProducts

        //    };
        //}

        public static DTOCategory CategoryToDto(Category category)
        {
            if(category == null) return new DTOCategory
            {
   
            };

            return new DTOCategory
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
                
                
            };
        }

        public static Category DtoToCategory(DTOCategory dto)
        {
            return new Category
            {

                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName

            };
        }

    }
}

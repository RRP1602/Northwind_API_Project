using NorthwindAPI.Models;
using NorthwindAPI.Services;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NorthwindAPI_Tests

{
    public class ServiceTests
    {
        private NorthwindContext _context;
        private IProductService _sut;
        
        [SetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "NorthwindDB").Options;
            _context = new NorthwindContext(options);
            _sut = new ProductService(_context);
           //  _sut.AddProductAsync(new Product {ProductID = 1, ProductName = "Freddo",SupplierID = 1,CategoryID = 1, QuantityPerUnit = "", UnitPrice = 1.50,UnitsInStock = 2,UnitsOnOrder = 1, ReorderLevel = 5}).Wait();

        }

        [Test]
        public void GivenProducts_GetAllProductsAsync_ReturnsAllProducts()
        {
            var result = _sut.GetAllProductsAsync();
            Assert.That(result, Is.TypeOf<List<Product>>());
        }

        [Test]
        public void GivenProduct_AddProductAsync_ReturnsNewProduct()
        {

            var result = _sut.AddProductAsync(new Product { ProductId = 10, ProductName = "Freddo", SupplierId = 10, CategoryId = 10, QuantityPerUnit = "132", UnitPrice = 10, UnitsInStock = 2, UnitsOnOrder = 1, ReorderLevel = 5 });
            Assert.That(result, Is.EqualTo(1.50));
        }

    }
}
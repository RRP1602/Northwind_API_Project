using NorthwindAPI.Models;
using NorthwindAPI.Services;
using System.Collections.Generic;

namespace NorthwindAPI_Tests

{
    public class Tests
    {
        private NorthwindContext _context;
        private IProductService _sut;

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
            Assert.That(result.Value(), Is.TypeOf<List<Product>>());
        }

        [Test]
        public void GivenProduct_ AddProductAsync_ReturnsNewProduct()
        {

            var result = _sut.AddProductAsync(new Product { });
            Assert.That(result.UnitPrice, Is.EqualTo(10))
        }

    }
}
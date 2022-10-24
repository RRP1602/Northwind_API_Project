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

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "NorthwindDB").Options;
            _context = new NorthwindContext(options);
            _sut = new ProductService(_context);
            _sut.AddProductAsync(new Product { ProductId = 1, ProductName = "Test", SupplierId = 1, CategoryId = 1, QuantityPerUnit = "1", UnitPrice = 1, UnitsInStock = 1, UnitsOnOrder = 1, ReorderLevel = 1 }).Wait();
            _sut.AddProductAsync(new Product { ProductId = 2, ProductName = "TestSameSupplierId", SupplierId = 14, CategoryId = 2, QuantityPerUnit = "2", UnitPrice = 2, UnitsInStock = 2, UnitsOnOrder = 2, ReorderLevel = 2 }).Wait();

        }

        [Test]
        [Category("GetAllProductsAsync")]
        public void GivenProducts_GetAllProductsAsync_ReturnsAllProducts()
        {
            var result = _sut.GetAllProductsAsync().Result;
            Assert.That(result, Is.TypeOf<List<Product>>());
            // more tests
        }

        [Category("AddProductAsync")]
        [Test]
        public void GivenProduct_AddProductAsync_ReturnsNewProduct()
        {

            _sut.AddProductAsync(new Product { ProductId = 10, ProductName = "Freddo", SupplierId = 10, CategoryId = 10, QuantityPerUnit = "132", UnitPrice = 10, UnitsInStock = 2, UnitsOnOrder = 1, ReorderLevel = 5 }).Wait();
            var result = _context.Products.Where(x => x.ProductId == 10).FirstOrDefault();
            Assert.That(result, Is.TypeOf<Product>());
            Assert.That(result.ProductName, Is.EqualTo("Freddo"));
            Assert.That(result.SupplierId, Is.EqualTo(10));
            Assert.That(result.CategoryId, Is.EqualTo(10));
            Assert.That(result.QuantityPerUnit, Is.EqualTo("132"));
            Assert.That(result.UnitPrice, Is.EqualTo(10));
            Assert.That(result.UnitsInStock, Is.EqualTo(2));
            Assert.That(result.UnitsOnOrder, Is.EqualTo(1));
            Assert.That(result.ReorderLevel, Is.EqualTo(5));

            _context.Products.Remove(_sut.GetProductByIdAsync(10).Result);
        }
        [Category("GetProductByIdAsync")]
        [Test]
        public void GivenValidProductId_GetProductByIdAsync_ReturnsProduct()
        {
            var result = _sut.GetProductByIdAsync(1).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Product>());
            Assert.That(result.ProductName, Is.EqualTo("Test"));
            Assert.That(result.SupplierId, Is.EqualTo(1));
            Assert.That(result.CategoryId, Is.EqualTo(1));
            Assert.That(result.QuantityPerUnit, Is.EqualTo("1"));
            Assert.That(result.UnitPrice, Is.EqualTo(1));
            Assert.That(result.UnitsInStock, Is.EqualTo(1));
            Assert.That(result.UnitsOnOrder, Is.EqualTo(1));
            Assert.That(result.ReorderLevel, Is.EqualTo(1));
        }
        [Category("GetProductByNameAsync")]
        [Test]
        public void GivenValidProductName_GetProductByNameAsync_ReturnsProduct()
        {
            var result = _sut.GetProductByNameAsync("Test").Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Product>());
            Assert.That(result.ProductName, Is.EqualTo("Test"));
            Assert.That(result.SupplierId, Is.EqualTo(1));
            Assert.That(result.CategoryId, Is.EqualTo(1));
            Assert.That(result.QuantityPerUnit, Is.EqualTo("1"));
            Assert.That(result.UnitPrice, Is.EqualTo(1));
            Assert.That(result.UnitsInStock, Is.EqualTo(1));
            Assert.That(result.UnitsOnOrder, Is.EqualTo(1));
            Assert.That(result.ReorderLevel, Is.EqualTo(1));
        }
        [Category("GetProductByCategoryIdAsync")]
        [Test]
        public void GivenValidCategoryId_GetProductByCategoryIdAsync_ReturnsProduct()
        {
            var result = _sut.GetProductByCategoryIdAsync(1).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<List<Product>>());
            // more tests
        }
        [Test]
        public void GivenValidSupplierId_GetProductBySupplierIdAsync_ReturnsProduct()
        {
            Assert.That(_sut.GetProductBySupplierIdAsync(1).Result, Is.Not.Null);
            Assert.That(_sut.GetProductBySupplierIdAsync(1).Result, Is.TypeOf<List<Product>>());
            var result = _context.Products.Where(x => x.SupplierId == 1).FirstOrDefault();
            Assert.That(result.ProductName, Is.EqualTo("Test"));
            //Assert.That(result.ProductName, Is.EqualTo("TestSameSupplierId"));
            Assert.That(result.SupplierId, Is.EqualTo(1));
            Assert.That(result.CategoryId, Is.EqualTo(1));
            Assert.That(result.QuantityPerUnit, Is.EqualTo("1"));
            Assert.That(result.UnitPrice, Is.EqualTo(1));
            Assert.That(result.UnitsInStock, Is.EqualTo(1));
            Assert.That(result.UnitsOnOrder, Is.EqualTo(1));
            Assert.That(result.ReorderLevel, Is.EqualTo(1));
            // more tests of different products but same supplierId
        }
        [Category("RemoveProductByIdAsync")]
        [Test]
        public void GivenValidProduct_RemoveProductAsync_RemovesProductFromDatabase()
        {
            var product = _sut.GetProductByIdAsync(2).Result;
            _sut.RemoveProductAsync(product);

            var result = _context.Products.Where(x => x.SupplierId == 2).FirstOrDefault();

            Assert.That(result, Is.EqualTo(null));
        }
        [Category("ProductExists")]
        [Test]
        public void GivenProductExists_ProductsExsits_ReturnsTrue()
        {
            var result = _sut.ProductsExists(1);
            Assert.That(result, Is.EqualTo(true));
        }
        [Category("ProductExists")]
        [Test]
        public void GivenProducDoesNottExist_ProductsExsits_ReturnsFalse()
        {
            var result = _sut.ProductsExists(1000);
            Assert.That(result, Is.EqualTo(false));
        }
        [Category("SaveChangesAsync")]
        [Test]
        public void SaveChangesAsync_SavesToTheDatabase()
        {
            var result = _sut.SaveChangesAsync().Result;

            var newProduct = new Product { ProductName = "TEST TEST" };
            _sut.SaveChangesAsync();

            Assert.That(result, Is.TypeOf<int>());
            Assert.That(_context.Products.Where(x => x.ProductName == "TEST TEST"), Is.Not.Null);
        }
    }
}
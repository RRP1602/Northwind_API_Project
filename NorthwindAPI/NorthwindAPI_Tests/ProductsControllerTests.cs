using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NorthwindAPI.Controllers;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;
using NorthwindAPI.Services;

namespace NorthwindAPI_Tests
{
    public class ProductsControllerTests
    {
        private ProductsController? _sut;

        [Test]
        [Category("Happy")]
        public void ProductsController_CanBe_Constructed()
        {
            var mockService = new Mock<IProductService>();
            _sut = new ProductsController(mockService.Object);
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
        }

        [Test]
        [Category("Happy")]
        public void GetProducts_Returns_Expected()
        {
            IEnumerable<Product> products = new List<Product>() { new Product() {ProductName="TESTTEST"} };

            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetAllProductsAsync())
                .Returns(Task.FromResult(products));

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProducts().Result.Value;

            mockService.Verify(ms => ms.GetAllProductsAsync(), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.InstanceOf<IEnumerable<DTOProduct>>());
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault().ProductName, Is.EqualTo("TESTTEST"));
        }

        [Test]
        [Category("Happy")]
        public void When_GetProduct_Given_GoodId_Returns_Expected()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" }));

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProduct(int.MaxValue).Result.Value;

            mockService.Verify(ms => ms.GetProductByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.InstanceOf<DTOProduct>());
            Assert.That(result.ProductName, Is.EqualTo("TESTTEST"));
        }

        [Test]
        [Category("Sad")]
        public void When_GetProduct_Given_BadId_Returns_Null()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((Product)null));

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProduct(It.IsAny<int>()).Result.Value;

            mockService.Verify(ms => ms.GetProductByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("Happy")]
        public void When_PutProduct_Given_ValidData_Returns_NoContent()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.SaveChangesAsync())
                .Returns(Task.FromResult(It.IsAny<int>()));
            mockService.Setup(ms => ms.GetProductByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" }));
            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result =
                (StatusCodeResult)_sut.PutProduct(It.IsAny<int>(), new DTOProduct() { ProductName = "TESTTEST" }).Result;

            mockService.Verify(ms => ms.SaveChangesAsync(), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        }

        [Test]
        [Category("Sad")]
        public void When_PutProduct_Given_MismatchedId_Returns_BadRequest()
        {
            var mockService = new Mock<IProductService>();

            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result =
                (StatusCodeResult)_sut.PutProduct(It.IsAny<int>(), new DTOProduct() { ProductId=1234, ProductName="TESTTEST" }).Result;

            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }
    
        [Test]
        [Category("Sad")]
        public void When_PutProduct_Given_ValidData_SaveChangesAsync_Throws_Returns_NotFound()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" }));
            mockService.Setup(ms => ms.SaveChangesAsync())
                .Throws<DbUpdateConcurrencyException>();
            mockService.Setup(ms => ms.ProductsExsits(It.IsAny<int>()))
                .Returns(false);

            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result = (StatusCodeResult)_sut.PutProduct(It.IsAny<int>(), new DTOProduct() { ProductName = "TESTTEST" }).Result;

            mockService.Verify(ms => ms.SaveChangesAsync(), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        [Category("Sad")]
        public void When_PutProduct_Given_ValidData_SaveChangesAsync_Throws_ThenThrows_AggregateException()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" }));
            mockService.Setup(ms => ms.SaveChangesAsync())
                .Throws<DbUpdateConcurrencyException>();
            mockService.Setup(ms => ms.ProductsExsits(It.IsAny<int>()))
                .Returns(true);

            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result = null;

            Assert.Throws<AggregateException>(() =>
            {
                result = (StatusCodeResult)_sut.PutProduct(It.IsAny<int>(), new DTOProduct() { ProductName = "TESTTEST" }).Result;
            });

            mockService.Verify(ms => ms.SaveChangesAsync(), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Null);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NorthwindAPI.Controllers;
using NorthwindAPI.Models;
using NorthwindAPI.Services;

namespace NorthwindAPI_Tests
{
    public class ProductsControllerTests
    {
        private ProductsController? _sut;

        [Test]
        public void ProductsController_CanBe_Constructed()
        {
            var mockService = new Mock<IProductService>();
            _sut = new ProductsController(mockService.Object);
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
        }

        [Test]
        [Category("GetProducts")]
        public void GetProducts_Returns_Expected()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetAllProductsAsync())
                .Returns(Task.FromResult(It.IsAny<IEnumerable<Product>>()));
            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProducts().Result.Value;

            mockService.Verify(ms => ms.GetAllProductsAsync(), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.InstanceOf<IEnumerable<Product>>());
            Assert.That(result, Is.EqualTo(It.IsAny<IEnumerable<Product>>()));
        }


        [Test]
        [Category("GetProduct")]
        public void GetProduct_Returns_Expected()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(It.IsAny<Product>()));

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProduct(It.IsAny<int>()).Result.Value;

            mockService.Verify(ms => ms.GetProductByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.InstanceOf<Product>());
            Assert.That(result, Is.EqualTo(It.IsAny<Product>()));
        }

        [Test]
        [Category("PutProduct")]
        public void When_PutProduct_Given_MismatchedId_Returns_BadRequest()
        {
            var mockService = new Mock<IProductService>();

            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result = 
                (StatusCodeResult)_sut.PutProduct(100, It.IsAny<Product>()).Result;

            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        [Category("PutProduct")]
        public void When_PutProduct_Given_ValidData_Returns_NoContent()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.SaveChangesAsync())
                .Returns(Task.FromResult(It.IsAny<int>()));
            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result =
                (StatusCodeResult)_sut.PutProduct(It.IsAny<int>(), It.IsAny<Product>()).Result;

            mockService.Verify(ms => ms.SaveChangesAsync(), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.EqualTo(new NoContentResult()));
        }


        [Test]
        [Category("PutProduct")]
        public void When_PutProduct_Given_ValidData_SaveChangesAsync_Throws_Returns_NotFound()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.SaveChangesAsync())
                .Returns(Task.FromResult(It.IsAny<int>()));
            mockService.Setup(ms => ms.SaveChangesAsync())
                .Throws<DbUpdateConcurrencyException>();

            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result = null;

            Assert.Throws<AggregateException>(() =>
            {
                result = (StatusCodeResult)_sut.PutProduct(int.MaxValue, It.IsAny<Product>()).Result;
            });

            mockService.Verify(ms => ms.SaveChangesAsync(), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.EqualTo(new NoContentResult()));
        }
    }
}
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
            Assert.That(result!.Count, Is.EqualTo(1));
            Assert.That(result!.FirstOrDefault()!.ProductName, Is.EqualTo("TESTTEST"));
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
            Assert.That(result!.ProductName, Is.EqualTo("TESTTEST"));
        }

        [Test]
        [Category("Sad")]
        public void When_GetProduct_Given_BadId_Returns_Null()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((Product)null!));

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
            mockService.Setup(ms => ms.ProductsExists(It.IsAny<int>()))
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
            mockService.Setup(ms => ms.ProductsExists(It.IsAny<int>()))
                .Returns(true);

            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result = null!;

            Assert.Throws<AggregateException>(() =>
            {
                result = (StatusCodeResult)_sut.PutProduct(It.IsAny<int>(), new DTOProduct() { ProductName = "TESTTEST" }).Result;
            });

            mockService.Verify(ms => ms.SaveChangesAsync(), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("Happy")]
        public void When_GetProductByName_Given_ValidString_Returns_Expected()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" })!);

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProductByName(It.IsAny<string>()).Result.Value;

            mockService.Verify(ms => ms.GetProductByNameAsync(It.IsAny<string>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ProductName, Is.EqualTo("TESTTEST"));
        }
        [Test]
        [Category("Sad")]
        public void When_GetProductByName_Given_InvalidString_Returns_Null()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((Product)null!)!);

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProductByName(It.IsAny<string>()).Result.Value;

            mockService.Verify(ms => ms.GetProductByNameAsync(It.IsAny<string>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("Happy")]
        public void When_GetProductByCategoryId_Given_ValidId_Returns_Expected()
        {
            IEnumerable<Product> expected = new List<Product>()
            {
                new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" }
            };

            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByCategoryIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(expected));

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProductByCategoryId(It.IsAny<int>()).Result.Value;

            mockService.Verify(ms => ms.GetProductByCategoryIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.FirstOrDefault()!.ProductName, Is.EqualTo("TESTTEST"));
        }

        [Test]
        [Category("Sad")]
        public void When_GetProductByCategoryId_Given_InvalidId_Returns_Null()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByCategoryIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(It.IsAny<IEnumerable<Product>>()));

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProductByCategoryId(It.IsAny<int>()).Result.Value;

            mockService.Verify(ms => ms.GetProductByCategoryIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("Happy")]
        public void When_GetProductBySupplierId_Given_ValidId_Returns_Expected()
        {
            IEnumerable<Product> expected = new List<Product>()
            {
                new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" }
            };

            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductBySupplierIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(expected));

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProductBySupplierId(It.IsAny<int>()).Result.Value;

            mockService.Verify(ms => ms.GetProductBySupplierIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.FirstOrDefault()!.ProductName, Is.EqualTo("TESTTEST"));
        }

        [Test]
        [Category("Sad")]
        public void When_GetProductBySupplierId_Given_InvalidId_Returns_Null()
        {
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductBySupplierIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(It.IsAny<IEnumerable<Product>>()));

            _sut = new ProductsController(mockService.Object);

            var result = _sut.GetProductBySupplierId(It.IsAny<int>()).Result.Value;

            mockService.Verify(ms => ms.GetProductBySupplierIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("Happy")]
        public void When_PostProduct_Given_ValidDTO_Returns_Expected()
        {
            var expected = new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" };
            var dto = new DTOProduct() { ProductId = int.MaxValue, ProductName = "TESTTEST" };
           
            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.AddProductAsync(It.IsAny<Product>())).Returns(Task.FromResult(expected));

            _sut = new ProductsController(mockService.Object);

            var result = _sut.PostProduct(dto).Result.Value;

            mockService.Verify(ms => ms.AddProductAsync(It.IsAny<Product>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("Happy")]
        public void When_DeleteProduct_Given_ValidId_Returns_Expected()
        {
            var expected = new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" };
            var dto = new DTOProduct() { ProductId = int.MaxValue, ProductName = "TESTTEST" };

            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(expected));
            mockService.Setup(ms => ms.RemoveProductAsync(It.IsAny<Product>()));

            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result = (StatusCodeResult)_sut.DeleteProduct(It.IsAny<int>()).Result;

            mockService.Verify(ms => ms.GetProductByIdAsync(It.IsAny<int>()), Times.Once());
            mockService.Verify(ms => ms.RemoveProductAsync(It.IsAny<Product>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        }

        [Test]
        [Category("Happy")]
        public void When_DeleteProduct_Given_ValidId_Returns_ExpectedAAA()
        {
            var expected = new Product() { ProductId = int.MaxValue, ProductName = "TESTTEST" };
            var dto = new DTOProduct() { ProductId = int.MaxValue, ProductName = "TESTTEST" };

            var mockService = new Mock<IProductService>();
            mockService.Setup(ms => ms.GetProductByIdAsync(It.IsAny<int>())).Returns(Task.FromResult((Product)null!));
            mockService.Setup(ms => ms.RemoveProductAsync(It.IsAny<Product>()));

            _sut = new ProductsController(mockService.Object);

            StatusCodeResult result = (StatusCodeResult)_sut.DeleteProduct(It.IsAny<int>()).Result;

            mockService.Verify(ms => ms.GetProductByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.That(_sut, Is.InstanceOf<ProductsController>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
    }
}
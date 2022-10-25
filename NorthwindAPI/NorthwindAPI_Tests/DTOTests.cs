using Moq;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;
using System.Text;

namespace NorthwindAPI_Tests
{
    public class DTOTests
    {
        [Test]
        public void DTOProduct_Works_As_Expected()
        {
            var result = new DTOProduct()
            {
                ProductId = 1,
                ProductName = "TEST",
                SupplierId = 1,
                CategoryId = 2,
                UnitPrice = 3
            }; 

            Assert.That(result, Is.Not.Null); 
            Assert.That(result, Is.InstanceOf<DTOProduct>());
            Assert.That(result.ProductId, Is.EqualTo(1));
            Assert.That(result.ProductName, Is.EqualTo("TEST"));
            Assert.That(result.SupplierId, Is.EqualTo(1));
            Assert.That(result.CategoryId, Is.EqualTo(2));
            Assert.That(result.UnitPrice, Is.EqualTo(3));
        }

        [Test]
        public void DTOSupplier_Works_As_Expected()
        {
            var result = new DTOSupplier()
            {
                SupplierId = 1,
                CompanyName = "TEST",
                ContactName = "TEST",
                ContactTitle = "TEST",
                Country = "TEST",
                TotalProducts = 1,
            };

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<DTOSupplier>());
            Assert.That(result.SupplierId, Is.EqualTo(1));
            Assert.That(result.CompanyName, Is.EqualTo("TEST"));
            Assert.That(result.ContactName, Is.EqualTo("TEST"));
            Assert.That(result.ContactTitle, Is.EqualTo("TEST"));
            Assert.That(result.Country, Is.EqualTo("TEST"));
            Assert.That(result.TotalProducts, Is.EqualTo(1));
        }

        [Test]
        public void DTOCategory_Works_As_Expected()
        {
            var result = new DTOCategory()
            {
                CategoryId = 1,
                CategoryName = "TEST",
                CategoryDescription = "TEST",
                TotalProducts = 1,
            };

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<DTOCategory>());
            Assert.That(result.CategoryId, Is.EqualTo(1));
            Assert.That(result.CategoryName, Is.EqualTo("TEST"));
            Assert.That(result.CategoryDescription, Is.EqualTo("TEST"));
            Assert.That(result.TotalProducts, Is.EqualTo(1));
        }

        [Test]
        public void DTOOrderDetails_Works_As_Expected()
        {
            var result = new DTOOrderDetails()
            {
                OrderId = 1,
                ProductId = 1,
                TotalPrice = 1,
                TotalDiscount = 1,
                TotalProducts = 1,
            };

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<DTOOrderDetails>());
            Assert.That(result.OrderId, Is.EqualTo(1));
            Assert.That(result.ProductId, Is.EqualTo(1));
            Assert.That(result.TotalPrice, Is.EqualTo(1));
            Assert.That(result.TotalDiscount, Is.EqualTo(1));
            Assert.That(result.TotalProducts, Is.EqualTo(1));
        }
    }
}

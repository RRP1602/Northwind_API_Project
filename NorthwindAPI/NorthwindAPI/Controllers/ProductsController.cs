using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;
using NorthwindAPI.Services;

namespace NorthwindAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOProduct>>> GetProducts()
        {
            var products = await _service.GetAllProductsAsync();
            var dto = products.Select(p => Utils.ProductToDto(p)).ToList();
            return dto;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DTOProduct>> GetProduct(int id)
        {
            var product = await _service.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Utils.ProductToDto(product);
        }

        // GET: api/Products/ByName/phone
        [HttpGet("ByName/{name}")]
        public async Task<ActionResult<DTOProduct>> GetProductByName(string name)
        {
            var product = await _service.GetProductByNameAsync(name);

            if (product == null)
            {
                return NotFound();
            }

            return Utils.ProductToDto(product);
        }

        // GET: api/Products/ByCategory/1
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<DTOProduct>>> GetProductByCategoryId(int categoryId)
        {
            var product = await _service.GetProductByCategoryIdAsync(categoryId);
            if (product == null)
            {
                return NotFound();
            }

            var dto = product.Select(p => Utils.ProductToDto(p)).ToList();           

            return dto;
        }
        // GET: api/Products/BySupplir/1
        [HttpGet("BySupplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<DTOProduct>>> GetProductBySupplierId(int supplierId)
        {
            var product = await _service.GetProductBySupplierIdAsync(supplierId);

            if (product == null)
            {
                return NotFound();
            }

            var dto = product.Select(p => Utils.ProductToDto(p)).ToList();           

            return dto;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, DTOProduct dto)
        {
            if (id != dto.ProductId)
            {
                return BadRequest();
            }

            var product = await _service.GetProductByIdAsync(id);


            product.CategoryId = dto.Category.CategoryId;
            product.ProductName = dto.ProductName ?? product.ProductName;
            product.SupplierId = dto.Supplier.SupplierId;
            product.UnitPrice = dto.UnitPrice ?? product.UnitPrice;

            //_service.Entry(product).State = EntityState.Modified;

            try
            {
                await _service.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(DTOProduct product)
        {
            await _service.AddProductAsync(Utils.DtoToProduct(product));

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _service.RemoveProductAsync(product);

            return NoContent();
        }

        // GET: api/Products/Discontinued
        [HttpGet("Discontinued")]
        public async Task<IEnumerable<DTOProduct>> GetDiscontinuedProducts()
        {
            var all = await _service.GetAllProductsAsync();
            var discontinued = all.ToList().Where(p => p.Discontinued).ToList();
            return discontinued.Select(p => Utils.ProductToDto(p)).ToList();
        }


        // GET: api/Products/HighestReorderLevel
        [HttpGet("HighestReorderLevel")]
        public async Task<IEnumerable<DTOProduct>> GetProductsWithHighestReorderLevel()
        {
            var all = await _service.GetAllProductsAsync();
            var reorder = all.ToList()
                .Select(p => p)
                .OrderByDescending(p => p.ReorderLevel)
                .ToList();

            var reordedF = reorder.TakeWhile(p => p.ReorderLevel == reorder.First().ReorderLevel);
            return reordedF.Select(p => Utils.ProductToDto(p)).ToList();
        }

        // GET: api/Products/HighestStock
        [HttpGet("HighestStock")]
        public async Task<IEnumerable<DTOProduct>> GetProductWithHighestStock()
        {
            var all = await _service.GetAllProductsAsync();
            var highest = all.ToList()
                .Select(p => p)
                .OrderByDescending(p => p.UnitsInStock)
                .ToList();
            var highestF = highest.TakeWhile(p => p.UnitsInStock == highest.First().UnitsInStock).ToList();
            return highestF.Select(p => Utils.ProductToDto(p)).ToList();
        }

        // GET: api/Products/LowestStock
        [HttpGet("LowestStock")]
        public async Task<IEnumerable<DTOProduct>> GetProductsWithLowestStock()
        {
            var all = await _service.GetAllProductsAsync();
            var lowest = all.ToList()
                .Select(p => p)
                .OrderBy(p => p.UnitsInStock)
                .ToList();

            var lowestF =  lowest.TakeWhile(p => p.UnitsInStock == lowest.First().UnitsInStock);
            return lowestF.Select(p => Utils.ProductToDto(p)).ToList();
        }

        public Task<IEnumerable<Product>> GetTop3SellingProducts()
        {
            return _service.GetTop3SellingProducts();
        }

        public async Task<IEnumerable<Product>> GetProductsInMostPopularCategory()
        {
            return await _service.GetProductsInMostPopularCategory();
        }

        public async Task<Product?> GetBestSellingProduct()
        {
            return await _service.GetBestSellingProduct();
        }

        private bool ProductExists(int id)
        {
            //return _service.Products.Any(e => e.ProductId == id);
            return _service.ProductsExists(id);
        }
    }
}

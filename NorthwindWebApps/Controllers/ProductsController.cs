namespace NorthwindWebApps.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Northwind.Services.Products;

#pragma warning disable CA2007
#pragma warning disable SA1600
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManagementService productManagementService;

        public ProductsController(IProductManagementService productManagementService)
        {
            this.productManagementService = productManagementService ?? throw new ArgumentNullException(nameof(productManagementService));
        }

        [HttpGet]
        public async IAsyncEnumerable<Product> GetAllProductsAsync()
        {
            await foreach (var product in this.productManagementService.ShowAllProductsAsync())
            {
                yield return product;
            }
        }

        [HttpGet("category/{id}")]
        public ActionResult<Product> GetProductForCategoryAsync(int id)
        {
            var product = this.productManagementService.ShowProductsForCategoryAsync(id);

            if (product != null)
            {
                return this.Ok(product);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            var product = await this.productManagementService.ShowProductAsync(id);

            if (product != null)
            {
                return this.Ok(product);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet("{offset}/{limit}")]
        public async IAsyncEnumerable<Product> GetProductsAsync(int offset, int limit)
        {
            if (offset < 0 || limit <= 0)
            {
                throw new ArgumentOutOfRangeException($"{offset} Cannot be less zero and {limit} cannot be as well");
            }

            await foreach (var product in this.productManagementService.ShowProductsAsync(offset, limit))
            {
                yield return product;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProductAsync(Product product)
        {
            if (product is null)
            {
                return this.BadRequest();
            }

            var id = await this.productManagementService.CreateProductAsync(product);

            if (id > 0)
            {
                product.Id = id;
                return this.CreatedAtAction(nameof(this.AddProductAsync), new { Id = id }, product);
            }
            else
            {
                return this.Conflict();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, Product product)
        {
            if (product is null || id <= 0)
            {
                return this.BadRequest();
            }

            if (await this.productManagementService.UpdateProductAsync(id, product))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            if (await this.productManagementService.DestroyProductAsync(id))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}

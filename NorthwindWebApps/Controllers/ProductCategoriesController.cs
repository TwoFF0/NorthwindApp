#pragma warning disable CA2007

using System;

namespace NorthwindWebApps.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Northwind.Services.Products;

    /// <summary>
    /// ProductCategoriesController.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryPictureService productCategoryPictureService;
        private readonly IProductCategoryManagementService productCategoryManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoriesController"/> class.
        /// </summary>
        /// <param name="productCategoryManagementService">ProductCategory service.</param>
        /// <exception cref="ArgumentNullException">Thrown if service is null.</exception>
        public ProductCategoriesController(IProductCategoryManagementService productCategoryManagementService, IProductCategoryPictureService productCategoryPictureService)
        {
            this.productCategoryManagementService = productCategoryManagementService ?? throw new ArgumentNullException(nameof(productCategoryManagementService));
            this.productCategoryPictureService = productCategoryPictureService ?? throw new ArgumentNullException(nameof(productCategoryPictureService));
        }

        /// <summary>
        /// Get all categories <see cref="ProductCategory"/>.
        /// </summary>
        /// <returns>All existed categories.</returns>
        [HttpGet]
        public async IAsyncEnumerable<ProductCategory> GetAllCategoriesAsync()
        {
            await foreach (var category in this.productCategoryManagementService.ShowAllCategoriesAsync())
            {
                yield return category;
            }
        }

        /// <summary>
        /// GET category with specified <see cref="id"/>.
        /// </summary>
        /// <param name="id">Id of category.</param>
        /// <returns>Specified category.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetCategoryAsync(int id)
        {
            var category = await this.productCategoryManagementService.ShowCategoryAsync(id);

            if (category != null)
            {
                return this.Ok(category);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// GET categories from <see cref="offset"/> to <see cref="limit"/>.
        /// </summary>
        /// <param name="offset">To start from.</param>
        /// <param name="limit">Limit of categories.</param>
        /// <returns>Numbers of categories in a range between <see cref="offset"/> + <see cref="limit"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="offset"/> or <see cref="limit"/> is invalid.</exception>
        [HttpGet("{offset}/{limit}")]
        public async IAsyncEnumerable<ProductCategory> GetCategoriesAsync(int offset, int limit)
        {
            if (offset < 0 || limit <= 0)
            {
                throw new ArgumentOutOfRangeException($"{offset} Cannot be less zero and {limit} cannot be as well");
            }

            await foreach (var category in this.productCategoryManagementService.ShowCategoriesAsync(offset, limit))
            {
                yield return category;
            }
        }

        /// <summary>
        /// POST category <see cref="ProductCategory"/> to a collection.
        /// </summary>
        /// <param name="category">CategoryEntity to POST.</param>
        /// <returns>BadRequest if category is null or <see cref="ProductCategory"/> if everything is ok.</returns>
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> AddCategoryAsync(ProductCategory category)
        {
            if (category is null)
            {
                return this.BadRequest();
            }

            var id = await this.productCategoryManagementService.CreateCategoryAsync(category);

            if (id > 0)
            {
                category.Id = id;
                return this.CreatedAtAction(nameof(this.AddCategoryAsync), new { Id = id }, category);
            }
            else
            {
                return this.Conflict();
            }
        }

        /// <summary>
        /// PUT new category instead existed one.
        /// </summary>
        /// <param name="id">Id of category to update.</param>
        /// <param name="category">CategoryEntity to update.</param>
        /// <returns>Bad request if category is null || NoContent -> everything OK || NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, ProductCategory category)
        {
            if (category is null || id <= 0)
            {
                return this.BadRequest();
            }

            if (await this.productCategoryManagementService.UpdateCategoriesAsync(id, category))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// DELETE category.
        /// </summary>
        /// <param name="id">Id of category to delete.</param>
        /// <returns>Bad request if category is null || NoContent -> everything OK || NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            if (await this.productCategoryManagementService.DestroyCategoryAsync(id))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// GET picture of category <see cref="categoryId"/>.
        /// </summary>
        /// <param name="categoryId">Id of category which belongs to picture.</param>
        /// <returns>If picture is null -> NotFound || File if all's good.</returns>
        [HttpGet("{categoryId}/picture")]
        public async Task<IActionResult> GetPicture(int categoryId)
        {
            var pic = await this.productCategoryPictureService.ShowPictureAsync(categoryId);

            if (pic is null)
            {
                return this.NotFound();
            }

            return this.File(pic, "image/png");
        }

        /// <summary>
        /// PUT picture.
        /// </summary>
        /// <param name="categoryId">Id of picture to update.</param>
        /// <param name="picture">New picture.</param>
        /// <returns>BadRequest if something wrong and NoContent if all's good.</returns>
        [HttpPut("{categoryId}/picture")]
        public async Task<IActionResult> UpdatePicture(int categoryId, IFormFile picture)
        {
            if (picture is null)
            {
                return this.BadRequest();
            }

            await using var memoryStream = new MemoryStream();
            await picture.CopyToAsync(memoryStream);

            if (await this.productCategoryPictureService.UpdatePictureAsync(categoryId, memoryStream))
            {
                return this.NoContent();
            }
            else
            {
                return this.BadRequest();
            }
        }

        /// <summary>
        /// DELETE picture.
        /// </summary>
        /// <param name="categoryId">Id of picture to delete.</param>
        /// <returns>NoContent if all's good and BadRequest if bad.</returns>
        [HttpDelete("{categoryId}/picture")]
        public async Task<IActionResult> DeletePicture(int categoryId)
        {
            if (await this.productCategoryPictureService.DestroyPictureAsync(categoryId))
            {
                return this.NoContent();
            }
            else
            {
                return this.BadRequest();
            }
        }
    }
}

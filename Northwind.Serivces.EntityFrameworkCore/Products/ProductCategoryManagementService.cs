namespace Northwind.Serivces.EntityFrameworkCore.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Northwind.Services.Products;

#pragma warning disable CA2007
#pragma warning disable SA1600
    public class ProductCategoryManagementService : IProductCategoryManagementService
    {
        private readonly NorthwindContext context;

        public ProductCategoryManagementService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategory> ShowAllCategoriesAsync()
        {
            await foreach (var productCategory in this.context.ProductCategories)
            {
                yield return productCategory;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategory> ShowCategoriesAsync(int offset, int limit)
        {
            var count = 0;

            await foreach (var productCategory in this.context.ProductCategories)
            {
                if (count >= offset && count < offset + limit)
                {
                    yield return productCategory;
                }

                count++;
            }
        }

        /// <inheritdoc/>
        public async Task<ProductCategory> ShowCategoryAsync(int categoryId)
        {
            return await this.context.ProductCategories.FindAsync(categoryId);
        }

        /// <inheritdoc/>
        public async Task<int> CreateCategoryAsync(ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            if (productCategory.Id is 0)
            {
                productCategory.Id = this.context.ProductCategories.Last().Id + 1;
            }

            await this.context.ProductCategories.AddAsync(productCategory);
            await this.context.SaveChangesAsync();

            return productCategory.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            var toDelete = await this.context.ProductCategories.FindAsync(categoryId);

            if (toDelete != null)
            {
                this.context.ProductCategories.Remove(toDelete);
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            var productCategoryToUpdate = await this.context.ProductCategories.FindAsync(categoryId);

            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate.Description = productCategory.Description;
                productCategoryToUpdate.Name = productCategory.Name;

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<ProductCategory> LookupCategoriesByNameAsync(IList<string> names)
        {
            throw new NotImplementedException();
        }
    }
}

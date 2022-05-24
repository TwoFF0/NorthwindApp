namespace Northwind.Services.DataAccess.Products
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Northwind.DataAccess;
    using Northwind.DataAccess.DAO_s;
    using Northwind.DataAccess.TransferObjects;
    using Northwind.Services.Products;

#pragma warning disable CA2007
#pragma warning disable SA1600
    public class ProductCategoriesManagementDataAccessService : IProductCategoryManagementService
    {
        private IProductCategoryDAO categoryDao;

        public ProductCategoriesManagementDataAccessService(NorthwindDataAccessFactory northwindData)
        {
            var northwind = northwindData ?? throw new ArgumentNullException(nameof(northwindData));
            this.categoryDao = northwind.GetProductCategoryDataAccessObject();
        }

        public async Task<int> CreateCategoryAsync(ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            return await this.categoryDao.InsertProductCategoryAsync(ToDTO(productCategory));
        }

        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            return await this.categoryDao.DeleteProductCategoryAsync(categoryId);
        }

        public async IAsyncEnumerable<ProductCategory> LookupCategoriesByNameAsync(IList<string> names)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            await foreach (var product in this.categoryDao.SelectProductCategoriesByNameAsync(names))
            {
                yield return ToObject(product);
            }
        }

        public async IAsyncEnumerable<ProductCategory> ShowAllCategoriesAsync()
        {
            await foreach (var product in this.categoryDao.SelectProductCategoriesAsync(0, 10))
            {
                yield return ToObject(product);
            }
        }

        public async IAsyncEnumerable<ProductCategory> ShowCategoriesAsync(int offset, int limit)
        {
            await foreach (var product in this.categoryDao.SelectProductCategoriesAsync(offset, limit))
            {
                yield return ToObject(product);
            }
        }

        public async Task<ProductCategory> ShowCategoryAsync(int categoryId)
        {
            return ToObject(await this.categoryDao.FindProductCategoryAsync(categoryId));
        }

        public async Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategory productCategory)
        {
            if (productCategory == null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            var dto = ToDTO(productCategory);

            return await this.categoryDao.UpdateProductCategoryAsync(dto);
        }

        private static ProductCategory ToObject(ProductCategoryTransferObject category)
        {
            return new ProductCategory()
            {
                Name = category.Name,
                Description = category.Description,
                Id = category.Id,
                Picture = category.Picture,
            };
        }

        private static ProductCategoryTransferObject ToDTO(ProductCategory category)
        {
            return new ProductCategoryTransferObject()
            {
                Name = category.Name,
                Description = category.Description,
                Id = category.Id,
                Picture = category.Picture,
            };
        }
    }
}

#pragma warning disable CA2007
#pragma warning disable SA1600

using System.Collections.Generic;

namespace Northwind.Services.DataAccess.Products
{
    using System;
    using System.Threading.Tasks;
    using Northwind.DataAccess;
    using Northwind.DataAccess.DAO_s;
    using Northwind.DataAccess.TransferObjects;
    using Northwind.Services.Products;

    public class ProductManagementDataAccessService : IProductManagementService
    {
        private IProductDAO productDao;

        public ProductManagementDataAccessService(NorthwindDataAccessFactory northwindData)
        {
            var northwind = northwindData ?? throw new ArgumentNullException(nameof(northwindData));
            this.productDao = northwind.GetProductDataAccessObject();
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            return await this.productDao.InsertProductAsync(ToTransferObject(product));
        }

        public async Task<bool> DestroyProductAsync(int productId)
        {
            return await this.productDao.DeleteProductAsync(productId);
        }

        public async IAsyncEnumerable<Product> LookupProductsByNameAsync(IList<string> names)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            await foreach (var product in this.productDao.SelectProductsByNameAsync(names))
            {
                yield return ToObject(product);
            }
        }

        public async IAsyncEnumerable<Product> ShowAllProductsAsync()
        {
            await foreach (var product in this.productDao.SelectProductsAsync(0, 10))
            {
                yield return ToObject(product);
            }
        }

        public async Task<Product> ShowProductAsync(int productId)
        {
            return ToObject(await this.productDao.FindProductAsync(productId));
        }

        public async IAsyncEnumerable<Product> ShowProductsAsync(int offset, int limit)
        {
            await foreach (var product in this.productDao.SelectProductsAsync(offset, limit))
            {
                yield return ToObject(product);
            }
        }

        public async IAsyncEnumerable<Product> ShowProductsForCategoryAsync(int categoryId)
        {
            var list = new List<int>() { categoryId };

            await foreach (var product in this.productDao.SelectProductByCategoryAsync(list))
            {
                yield return ToObject(product);
            }
        }

        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var dto = ToTransferObject(product);

            return await this.productDao.UpdateProductAsync(dto);
        }

        private static ProductTransferObject ToTransferObject(Product product)
        {
            return new ProductTransferObject()
            {
                Name = product.Name,
                Discontinued = product.Discontinued,
                QuantityPerUnit = product.QuantityPerUnit,
                CategoryId = product.CategoryId,
                Id = product.Id,
                ReorderLevel = product.ReorderLevel,
                SupplierId = product.SupplierId,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
            };
        }

        private static Product ToObject(ProductTransferObject product)
        {
            return new Product()
            {
                Name = product.Name,
                Discontinued = product.Discontinued,
                QuantityPerUnit = product.QuantityPerUnit,
                CategoryId = product.CategoryId,
                Id = product.Id,
                ReorderLevel = product.ReorderLevel,
                SupplierId = product.SupplierId,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
            };
        }
    }
}

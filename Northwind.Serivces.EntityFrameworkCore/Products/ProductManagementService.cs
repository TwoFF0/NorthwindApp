namespace Northwind.Serivces.EntityFrameworkCore.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Northwind.Services.Products;

#pragma warning disable CA2007
#pragma warning disable SA1600
#pragma warning disable CS1998
    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private readonly NorthwindContext context;

        public ProductManagementService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> ShowAllProductsAsync()
        {
            await foreach (var product in this.context.Products)
            {
                yield return product;
            }
        }

        /// <inheritdoc/>
        public async Task<Product> ShowProductAsync(int productId) => await this.context.Products.FindAsync(productId);

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> ShowProductsAsync(int offset, int limit)
        {
            var count = 0;

            await foreach (var product in this.context.Products)
            {
                if (count >= offset && count < offset + limit)
                {
                    yield return product;
                }

                count++;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Product> ShowProductsForCategoryAsync(int categoryId)
        {
            var products = this.context.Products.Where(x => x.CategoryId == categoryId);

            foreach (var product in products)
            {
                yield return product;
            }
        }

        /// <inheritdoc/>
        public async Task<int> CreateProductAsync(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (product.Id is 0)
            {
                product.Id = this.context.Products.Max(x => x.Id) + 1;
            }

            await this.context.Products.AddAsync(product);
            await this.context.SaveChangesAsync();

            return product.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var productToUpdate = await this.context.Products.FindAsync(productId);

            if (productToUpdate != null)
            {
                productToUpdate.Name = product.Name;
                productToUpdate.QuantityPerUnit = product.QuantityPerUnit;
                productToUpdate.CategoryId = product.CategoryId;
                productToUpdate.Discontinued = product.Discontinued;
                productToUpdate.ReorderLevel = product.ReorderLevel;
                productToUpdate.SupplierId = product.SupplierId;
                productToUpdate.UnitPrice = product.UnitPrice;
                productToUpdate.UnitsInStock = product.UnitsInStock;
                productToUpdate.UnitsOnOrder = product.UnitsOnOrder;

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyProductAsync(int productId)
        {
            var toDelete = await this.context.Products.FindAsync(productId);

            if (toDelete != null)
            {
                this.context.Products.Remove(toDelete);
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<Product> LookupProductsByNameAsync(IList<string> names)
        {
            throw new NotImplementedException();
        }
    }
}

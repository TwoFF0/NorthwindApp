﻿namespace Northwind.Services.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

#pragma warning disable SA1600

    public interface IProductCategoryManagementService
    {
        /// <summary>
        /// Shows a list of product categories.
        /// </summary>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ProductCategory"/>.</returns>
        IAsyncEnumerable<ProductCategory> ShowAllCategoriesAsync();

        /// <summary>
        /// Try to show a product category with specified identifier.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>Returns true if a product category is returned; otherwise false.</returns>
        Task<ProductCategory> ShowCategoryAsync(int categoryId);

        /// <summary>
        /// Shows a list of product categories using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ProductCategory"/>.</returns>
        IAsyncEnumerable<ProductCategory> ShowCategoriesAsync(int offset, int limit);

        /// <summary>
        /// Creates a new product category.
        /// </summary>
        /// <param name="productCategory">A <see cref="ProductCategory"/> to create.</param>
        /// <returns>An identifier of a created product category.</returns>
        Task<int> CreateCategoryAsync(ProductCategory productCategory);

        /// <summary>
        /// Updates a product category.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <param name="productCategory">A <see cref="ProductCategory"/>.</param>
        /// <returns>True if a product category is updated; otherwise false.</returns>
        Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategory productCategory);

        /// <summary>
        /// Destroys an existed product category.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>True if a product category is destroyed; otherwise false.</returns>
        Task<bool> DestroyCategoryAsync(int categoryId);

        /// <summary>
        /// Looks up for product categories with specified names.
        /// </summary>
        /// <param name="names">A list of product category names.</param>
        /// <returns>A list of product categories with specified names.</returns>
        IAsyncEnumerable<ProductCategory> LookupCategoriesByNameAsync(IList<string> names);
    }
}

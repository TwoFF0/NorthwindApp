namespace Northwind.Services.EntityFrameworkCore.Blogging.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Northwind.Services.Blogging.Models;
    using Northwind.Services.Blogging.Services;
    using Northwind.Services.EntityFrameworkCore.Blogging.Context;
    using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

#pragma warning disable CA2007

    /// <summary>
    /// Services for product link in blogs.
    /// </summary>
    public class BloggingProductLinkService : IBloggingProductLinkService
    {
        private readonly BloggingContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingProductLinkService"/> class.
        /// </summary>
        /// <param name="designTimeDbContextFactory">Factory to initialize context.</param>
        /// <param name="mapper">Mapper to map entities.</param>
        public BloggingProductLinkService(IDesignTimeDbContextFactory<BloggingContext> designTimeDbContextFactory, IMapper mapper)
        {
            var factory = designTimeDbContextFactory ??
                          throw new ArgumentNullException(nameof(designTimeDbContextFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = factory.CreateDbContext(null!);
        }

        /// <summary>
        /// Adding product link to blog.
        /// </summary>
        /// <param name="articleId">Blog to add product link.</param>
        /// <param name="productId">Product id to add.</param>
        /// <returns><see cref="BlogArticleProduct"/>.</returns>
        public async Task<BlogArticleProduct> AddProductLinkAsync(int articleId, int productId)
        {
            var blogArticleProduct = new BlogArticleProductEntity()
            {
                BlogArticleId = articleId,
                ProductId = productId,
            };

            await this.context.BlogProducts.AddAsync(blogArticleProduct);
            await this.context.SaveChangesAsync();

            return this.mapper.Map<BlogArticleProduct>(blogArticleProduct);
        }

        /// <summary>
        /// Delete product link in blog.
        /// </summary>
        /// <param name="articleId">Blog id to add link.</param>
        /// <param name="productId">Id of product.</param>
        /// <returns>True if all's good, otherwise false.</returns>
        public async Task<bool> DeleteProductLinkAsync(int articleId, int productId)
        {
            var entity = await this.context.BlogProducts.
                Where(x => x.BlogArticleId == articleId && x.ProductId == productId).
                FirstOrDefaultAsync();

            if (entity == null)
            {
                return false;
            }

            this.context.BlogProducts.Remove(entity);
            await this.context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Returns all product links.
        /// </summary>
        /// <returns>Returns all <see cref="BlogArticleProduct"/>.</returns>
        public async IAsyncEnumerable<BlogArticleProduct> GetBlogProductsAsync()
        {
            await foreach (var product in this.context.BlogProducts)
            {
                yield return this.mapper.Map<BlogArticleProduct>(product);
            }
        }
    }
}

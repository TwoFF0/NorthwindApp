namespace Northwind.Services.EntityFrameworkCore.Blogging.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore.Design;
    using Northwind.Services.Blogging.Models;
    using Northwind.Services.Blogging.Services;
    using Northwind.Services.EntityFrameworkCore.Blogging.Context;
    using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

#pragma warning disable CA2007

    /// <summary>
    /// Service to work with blogging.
    /// </summary>
    public class BloggingService : IBloggingService
    {
        private readonly BloggingContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingService"/> class.
        /// </summary>
        /// <param name="designTimeDbContextFactory">Factory to initialize context.</param>
        /// <param name="mapper">Mapper to map entities.</param>
        public BloggingService(IDesignTimeDbContextFactory<BloggingContext> designTimeDbContextFactory, IMapper mapper)
        {
            var factory = designTimeDbContextFactory ??
                          throw new ArgumentNullException(nameof(designTimeDbContextFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = factory.CreateDbContext(null!);
        }

        /// <summary>
        /// Adding blog.
        /// </summary>
        /// <param name="blogArticle">Blog to add.</param>
        /// <returns>Id of added article.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="BlogArticle"/> is null.</exception>
        public async Task<int> AddBlogArticleAsync(BlogArticle blogArticle)
        {
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

            var entity = this.mapper.Map<BlogArticleEntity>(blogArticle);

            entity.Posted = DateTime.Now;
            await this.context.BlogArticles.AddAsync(entity);
            await this.context.SaveChangesAsync();

            return entity.BlogArticleId;
        }

        /// <summary>
        /// Delete blog.
        /// </summary>
        /// <param name="blogArticleId">Blog id to delete.</param>
        /// <returns>True if all's good, otherwise false.</returns>
        public async Task<bool> DeleteBlogArticleAsync(int blogArticleId)
        {
            var entity = await this.context.BlogArticles.FindAsync(blogArticleId);

            if (entity == null)
            {
                return false;
            }

            this.context.BlogArticles.Remove(entity);
            await this.context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Updates blog.
        /// </summary>
        /// <param name="blogArticle">New blog.</param>
        /// <param name="blogArticleId">Blog id to update.</param>
        /// <returns>True if all's good, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="BlogArticle"/> is null.</exception>
        public async Task<bool> UpdateBlogArticleAsync(BlogArticle blogArticle, int blogArticleId)
        {
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

            var entity = await this.context.BlogArticles.FindAsync(blogArticleId);

            if (entity == null)
            {
                return false;
            }

            entity.Content = blogArticle.Content;
            entity.Title = blogArticle.Title;
            entity.Posted = DateTime.Now;

            await this.context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Gets all blogs.
        /// </summary>
        /// <returns>Returns all existed <see cref="BlogArticle"/>.</returns>
        public async IAsyncEnumerable<BlogArticle> GetAllBlogsAsync()
        {
            await foreach (var blogArticle in this.context.BlogArticles)
            {
                yield return this.mapper.Map<BlogArticle>(blogArticle);
            }
        }

        /// <summary>
        /// Gets blog by id.
        /// </summary>
        /// <param name="blogId">Id of blog to find.</param>
        /// <returns>Founded blog.</returns>
        public async Task<BlogArticle> GetBlogArticleAsync(int blogId) =>
            this.mapper.Map<BlogArticle>(await this.context.BlogArticles.FindAsync(blogId));
    }
}
namespace Northwind.Services.EntityFrameworkCore.Blogging.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore.Design;
    using Northwind.Services.Blogging.Models;
    using Northwind.Services.Blogging.Services;
    using Northwind.Services.EntityFrameworkCore.Blogging.Context;
    using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

#pragma warning disable CA2007

    /// <summary>
    /// Service for <see cref="BlogArticle"/> comments.
    /// </summary>
    public class BloggingCommentService : IBloggingCommentService
    {
        private readonly IMapper mapper;
        private readonly BloggingContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingCommentService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper to map entities.</param>
        /// <param name="designTimeDbContextFactory">Factory to initialize context.</param>
        public BloggingCommentService(IMapper mapper, IDesignTimeDbContextFactory<BloggingContext> designTimeDbContextFactory)
        {
            var factory = designTimeDbContextFactory ?? throw new ArgumentNullException(nameof(designTimeDbContextFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = factory.CreateDbContext(null!);
        }

        /// <summary>
        /// Adding comment in <see cref="BlogArticleComment"/>.
        /// </summary>
        /// <param name="comment">Comment to add.</param>
        /// <returns>Id of added comments.</returns>
        public async Task<int> CreateComment(BlogArticleComment comment)
        {
            if (comment is null)
            {
                return -1;
            }

            await this.context.BlogComments.AddAsync(this.mapper.Map<BlogArticleCommentEntity>(comment));
            await this.context.SaveChangesAsync();

            return this.context.BlogComments.Max(x => x.Id);
        }

        /// <summary>
        /// Returns all comments.
        /// </summary>
        /// <returns>Returns all <see cref="BlogArticleComment"/> of article.</returns>
        public async IAsyncEnumerable<BlogArticleComment> GetComments()
        {
            await foreach (var comment in this.context.BlogComments)
            {
                yield return this.mapper.Map<BlogArticleComment>(comment);
            }
        }

        /// <summary>
        /// Updates comment.
        /// </summary>
        /// <param name="commentId">Id of comment to update.</param>
        /// <param name="comment">New comment.</param>
        /// <returns>True if all's good, otherwise false.</returns>
        public async Task<bool> UpdateComment(int commentId, string comment)
        {
            var foundedComment = await this.context.BlogComments.FindAsync(commentId);

            if (foundedComment is null)
            {
                return false;
            }

            foundedComment.Comment = comment;
            await this.context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Delete comment in the article.
        /// </summary>
        /// <param name="id">Id of comment.</param>
        /// <returns>True if all's good, otherwise false.</returns>
        public async Task<bool> DeleteComment(int id)
        {
            var foundedComment = await this.context.BlogComments.FindAsync(id);

            if (foundedComment is null)
            {
                return false;
            }

            this.context.BlogComments.Remove(foundedComment);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}

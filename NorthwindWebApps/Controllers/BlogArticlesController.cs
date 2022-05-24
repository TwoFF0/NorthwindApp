namespace NorthwindWebApps.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Northwind.Services;
    using Northwind.Services.Blogging;
    using Northwind.Services.Blogging.Models;
    using Northwind.Services.Blogging.Services;
    using Northwind.Services.Customers;
    using Northwind.Services.Employees;
    using Northwind.Services.Products;
    using NorthwindWebApps.ExtendedModels;

#pragma warning disable CA2007

    /// <summary>
    /// Controller of blog articles.
    /// </summary>
    [Route("api/articles")]
    [ApiController]
    public class BlogArticlesController : ControllerBase
    {
        private readonly IBloggingService bloggingService;
        private readonly IBloggingCommentService bloggingCommentService;
        private readonly IBloggingProductLinkService bloggingProductLinkService;
        private readonly IEmployeeManagementService employeeManagementService;
        private readonly IProductManagementService productManagementService;
        private readonly ICustomerService customerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticlesController"/> class.
        /// </summary>
        /// <param name="bloggingService">Blogging service.</param>
        /// <param name="employeeManagementService">Employee service.</param>
        /// <param name="productManagementService">Product service.</param>
        /// <param name="bloggingCommentService">Blogging comment service.</param>
        /// <param name="customerService">Customer service.</param>
        /// <param name="bloggingProductLinkService">Blogging product link service.</param>
        public BlogArticlesController(IBloggingService bloggingService, IEmployeeManagementService employeeManagementService, IProductManagementService productManagementService, IBloggingCommentService bloggingCommentService, ICustomerService customerService, IBloggingProductLinkService bloggingProductLinkService)
        {
            this.bloggingService = bloggingService ?? throw new ArgumentNullException(nameof(bloggingService));
            this.employeeManagementService = employeeManagementService ?? throw new ArgumentNullException(nameof(employeeManagementService));
            this.productManagementService = productManagementService ?? throw new ArgumentNullException(nameof(productManagementService));
            this.bloggingCommentService = bloggingCommentService ?? throw new ArgumentNullException(nameof(bloggingCommentService));
            this.customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            this.bloggingProductLinkService = bloggingProductLinkService ?? throw new ArgumentNullException(nameof(bloggingProductLinkService));
        }

        /// <summary>
        /// Post new blog.
        /// </summary>
        /// <param name="blogArticle">Data <see cref="BlogArticle"/> to post.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        public async Task<ActionResult<BlogArticle>> PostBlog(BlogArticle blogArticle)
        {
            if (blogArticle is null || blogArticle.EmployeeID <= 0)
            {
                return this.BadRequest();
            }

            if (await this.employeeManagementService.ShowEmployeeAsync(blogArticle.EmployeeID) == null)
            {
                return this.BadRequest();
            }

            var id = await this.bloggingService.AddBlogArticleAsync(blogArticle);

            if (id == -1)
            {
                return this.Conflict();
            }

            blogArticle.BlogArticleId = id;

            return this.CreatedAtAction(nameof(this.PostBlog), new { Id = id }, blogArticle);
        }

        /// <summary>
        /// Delete blog.
        /// </summary>
        /// <param name="id">Id of blog to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogArticle(int id)
        {
            if (!await this.bloggingService.DeleteBlogArticleAsync(id))
            {
                return this.BadRequest();
            }

            return this.NoContent();
        }

        /// <summary>
        /// Update existed blog.
        /// </summary>
        /// <param name="blogArticle">New blog data.</param>
        /// <param name="id">Id of blog to update.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogArticle(BlogArticle blogArticle, int id)
        {
            if (blogArticle is null)
            {
                return this.BadRequest();
            }

            var isUpdated = await this.bloggingService.UpdateBlogArticleAsync(blogArticle, id);

            if (!isUpdated)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <summary>
        /// Get single blog by id.
        /// </summary>
        /// <param name="id">Id of blog to represent.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogArticleFullInfo>> GetBlog(int id)
        {
            var blog = await this.bloggingService.GetBlogArticleAsync(id);

            if (blog == null || blog.EmployeeID <= 0)
            {
                return this.NotFound();
            }

            var employee = await this.employeeManagementService.ShowEmployeeAsync(blog.EmployeeID);

            if (employee == null)
            {
                return this.NotFound();
            }

            var fullBlog = new BlogArticleFullInfo(employee, blog);

            return this.Ok(fullBlog);
        }

        /// <summary>
        /// Gets all blogs.
        /// </summary>
        /// <returns>All existed blogs.</returns>
        [HttpGet]
        public async IAsyncEnumerable<BlogArticleShortInfo> GetBlogs()
        {
            await foreach (var blog in this.bloggingService.GetAllBlogsAsync())
            {
                var employee = await this.employeeManagementService.ShowEmployeeAsync(blog.EmployeeID);

                yield return new BlogArticleShortInfo(employee, blog);
            }
        }

        /// <summary>
        /// Post product link to existed blog.
        /// </summary>
        /// <param name="articleId"><see cref="BlogArticle"/> id.</param>
        /// <param name="productId"><see cref="Product"/> id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("{articleId}/products/{productId}")]
        public async Task<ActionResult<BlogArticleProduct>> PostProductLink(int articleId, int productId)
        {
            var exists = await this.CheckIfExistsProductAndArticle(articleId, productId);

            if (exists is not OkResult)
            {
                return this.BadRequest();
            }

            var blogArticleProduct = await this.bloggingProductLinkService.AddProductLinkAsync(articleId, productId);

            return this.CreatedAtAction(nameof(this.PostProductLink), new { Id = blogArticleProduct.Id }, blogArticleProduct);
        }

        /// <summary>
        /// Delete product link to existed blog.
        /// </summary>
        /// <param name="articleId"><see cref="BlogArticle"/> id.</param>
        /// <param name="productId"><see cref=""/><see cref="Product"/> id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("{articleId}/products/{productId}")]
        public async Task<IActionResult> DeleteProductLink(int articleId, int productId)
        {
            var exists = await this.CheckIfExistsProductAndArticle(articleId, productId);

            if (exists is not OkResult)
            {
                return this.BadRequest();
            }

            if (!await this.bloggingProductLinkService.DeleteProductLinkAsync(articleId, productId))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <summary>
        /// Get all product links attached to <see cref="BlogArticle"/>.
        /// </summary>
        /// <param name="articleId"><see cref="BlogArticle"/> id.</param>
        /// <returns>All links attached to <see cref="BlogArticle"/>.</returns>
        [HttpGet("{articleId}/products")]
        public async IAsyncEnumerable<BlogArticleProduct> GetArticleProductLinks(int articleId)
        {
            var blog = await this.bloggingService.GetBlogArticleAsync(articleId);

            if (blog != null)
            {
                await foreach (var product in this.bloggingProductLinkService.GetBlogProductsAsync())
                {
                    yield return product;
                }
            }
        }

        /// <summary>
        /// Post comment in blog.
        /// </summary>
        /// <param name="articleId">Id of article.</param>
        /// <param name="comment"><see cref="BlogArticleComment"/> to post.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("{articleId}/comments")]
        public async Task<ActionResult<BlogArticleComment>> PostComment(int articleId, BlogArticleComment comment)
        {
            if (comment is null)
            {
                return this.BadRequest();
            }

            var article = await this.bloggingService.GetBlogArticleAsync(articleId);

            if (article is null)
            {
                return this.NotFound();
            }

            var customer = await this.customerService.GetCustomerAsync(comment.CustomerId);

            if (customer is null)
            {
                return this.NotFound();
            }

            comment.BlogArticleId = articleId;

            var id = await this.bloggingCommentService.CreateComment(comment);
            comment.Id = id;

            if (id <= 0)
            {
                return this.BadRequest();
            }

            return this.CreatedAtAction(nameof(this.PostComment), new { Id = id }, comment);
        }

        /// <summary>
        /// Get all comments in the blog.
        /// </summary>
        /// <param name="articleId">Id of article where to find comments.</param>
        /// <returns>Get all comments of article.</returns>
        [HttpGet("{articleId}/comments")]
        public async IAsyncEnumerable<BlogArticleComment> GetComments(int articleId)
        {
            var article = await this.bloggingService.GetBlogArticleAsync(articleId);

            if (article is not null)
            {
                await foreach (var comment in this.bloggingCommentService.GetComments())
                {
                    yield return comment;
                }
            }
        }

        /// <summary>
        /// Update comment in blog.
        /// </summary>
        /// <param name="articleId">Id of <see cref="BlogArticle"/> to update.</param>
        /// <param name="commentId">Id of comment to update.</param>
        /// <param name="comment">New comment.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("{articleId}/comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(int articleId, int commentId, string comment)
        {
            var article = await this.bloggingService.GetBlogArticleAsync(articleId);

            if (article is null)
            {
                return this.BadRequest();
            }

            if (!await this.bloggingCommentService.UpdateComment(commentId, comment))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <summary>
        /// Delete comment of <see cref="BlogArticle"/>.
        /// </summary>
        /// <param name="articleId">Id of <see cref="BlogArticle"/>.</param>
        /// <param name="commentId">Id of comment in the blog.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("{articleId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int articleId, int commentId)
        {
            var article = await this.bloggingService.GetBlogArticleAsync(articleId);

            if (article is null)
            {
                return this.BadRequest();
            }

            if (!await this.bloggingCommentService.DeleteComment(commentId))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        private async Task<IActionResult> CheckIfExistsProductAndArticle(int articleId, int productId)
        {
            var blog = await this.bloggingService.GetBlogArticleAsync(articleId);

            if (blog == null)
            {
                return this.NotFound();
            }

            var product = await this.productManagementService.ShowProductAsync(productId);

            if (product == null)
            {
                return this.NotFound();
            }

            return this.Ok();
        }
    }
}

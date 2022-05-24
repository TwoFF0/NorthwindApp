namespace Northwind.Services.Blogging.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Northwind.Services.Blogging.Models;

    public interface IBloggingService
    {
        public Task<int> AddBlogArticleAsync(BlogArticle blogArticle);

        public Task<bool> DeleteBlogArticleAsync(int blogArticleId);

        public Task<bool> UpdateBlogArticleAsync(BlogArticle blogArticle, int blogArticleId);

        public IAsyncEnumerable<BlogArticle> GetAllBlogsAsync();

        public Task<BlogArticle> GetBlogArticleAsync(int blogId);
    }
}

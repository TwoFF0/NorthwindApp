namespace Northwind.Services.Blogging.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Northwind.Services.Blogging.Models;

    public interface IBloggingProductLinkService
    {
        public Task<BlogArticleProduct> AddProductLinkAsync(int articleId, int productId);

        public IAsyncEnumerable<BlogArticleProduct> GetBlogProductsAsync();

        public Task<bool> DeleteProductLinkAsync(int articleId, int productId);
    }
}

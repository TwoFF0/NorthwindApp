namespace Northwind.Services.Blogging.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Northwind.Services.Blogging.Models;

    public interface IBloggingCommentService
    {
        public Task<int> CreateComment(BlogArticleComment comment);

        public IAsyncEnumerable<BlogArticleComment> GetComments();

        public Task<bool> UpdateComment(int commentId, string comment);

        public Task<bool> DeleteComment(int id);
    }
}

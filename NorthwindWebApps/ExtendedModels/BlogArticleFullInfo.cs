namespace NorthwindWebApps.ExtendedModels
{
    using System;
    using Northwind.Services.Blogging;
    using Northwind.Services.Blogging.Models;
    using Northwind.Services.Employees;

#pragma warning disable SA1600

    /// <summary>
    /// Represent extended full version of BlogArticleEntity <see cref="BlogArticle"/>.
    /// </summary>
    public class BlogArticleFullInfo
    {
        public BlogArticleFullInfo(Employee author, BlogArticle article)
        {
            if (author is null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            if (article is null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            this.Id = article.BlogArticleId;
            this.Title = article.Title;
            this.Content = article.Content;
            this.Posted = article.Posted;

            this.AuthorId = author.Id;
            this.AuthorName = author.Title == null
                ? $"{author.FirstName} {author.LastName}"
                : $"{author.FirstName} {author.LastName}, {author.Title}";
        }

        public int Id { get; }

        public string Title { get; }

        public string Content { get; }

        public DateTime Posted { get; }

        public int AuthorId { get; }

        public string AuthorName { get; }
    }
}

namespace Northwind.Services.Blogging.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class BlogArticle
    {
        [JsonIgnore]
        public int BlogArticleId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        [JsonIgnore]
        public DateTime Posted { get; set; }

        public int EmployeeID { get; set; }

        [JsonIgnore]
        public ICollection<BlogArticleProduct> Products { get; set; }

        [JsonIgnore]
        public ICollection<BlogArticleComment> Comments { get; set; }
    }
}

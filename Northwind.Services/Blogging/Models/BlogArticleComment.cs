namespace Northwind.Services.Blogging.Models
{
    using System.Text.Json.Serialization;

    public class BlogArticleComment
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int BlogArticleId { get; set; }

        public string CustomerId { get; set; }

        public string Comment { get; set; }
    }
}

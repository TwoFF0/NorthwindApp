namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable SA1600

    public class BlogArticleCommentEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        public int BlogArticleId { get; set; }

        public string CustomerId { get; set; }

        public string Comment { get; set; }
    }
}

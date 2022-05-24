namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BlogArticleProductEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int BlogArticleId { get; set; }
    }
}

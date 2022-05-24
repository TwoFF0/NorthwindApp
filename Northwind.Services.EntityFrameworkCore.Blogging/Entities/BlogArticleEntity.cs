namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
#pragma warning disable SA1600

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BlogArticleEntity
    {
        [Key]
        [Column("id")]
        public int BlogArticleId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Posted { get; set; }

        public int EmployeeID { get; set; }

        public ICollection<BlogArticleProductEntity> Products { get; }

        public ICollection<BlogArticleCommentEntity> Comments { get; }
    }
}

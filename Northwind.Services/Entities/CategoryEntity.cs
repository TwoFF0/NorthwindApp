#nullable disable

namespace Northwind.Services.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Index(nameof(CategoryName), Name = "CategoryName")]
    public partial class CategoryEntity
    {
        public CategoryEntity()
        {
            this.Products = new HashSet<ProductEntity>();
        }

        [Key]
        [Column("CategoryID")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        [InverseProperty(nameof(ProductEntity.CategoryEntity))]
        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}

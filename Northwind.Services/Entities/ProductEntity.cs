#nullable disable

namespace Northwind.Services.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Index(nameof(CategoryId), Name = "CategoriesProducts")]
    [Index(nameof(CategoryId), Name = "CategoryID")]
    [Index(nameof(ProductName), Name = "ProductName")]
    [Index(nameof(SupplierId), Name = "SupplierID")]
    [Index(nameof(SupplierId), Name = "SuppliersProducts")]
    public partial class ProductEntity
    {
        public ProductEntity()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        [Column("SupplierID")]
        public int? SupplierId { get; set; }

        [Column("CategoryID")]
        public int? CategoryId { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Products")]
        public virtual CategoryEntity CategoryEntity { get; set; }

        [ForeignKey(nameof(SupplierId))]
        [InverseProperty("Products")]
        public virtual Supplier Supplier { get; set; }

        [InverseProperty(nameof(OrderDetail.ProductEntity))]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

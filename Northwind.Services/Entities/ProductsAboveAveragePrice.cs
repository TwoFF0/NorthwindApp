#nullable disable

namespace Northwind.Services.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public partial class ProductsAboveAveragePrice
    {
        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }
    }
}

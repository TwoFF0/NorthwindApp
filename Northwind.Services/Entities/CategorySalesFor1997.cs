#nullable disable

namespace Northwind.Services.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public partial class CategorySalesFor1997
    {
        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [Column(TypeName = "money")]
        public decimal? CategorySales { get; set; }
    }
}

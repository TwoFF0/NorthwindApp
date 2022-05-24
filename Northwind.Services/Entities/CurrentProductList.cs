#nullable disable

namespace Northwind.Services.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public partial class CurrentProductList
    {
        [Column("ProductID")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }
    }
}

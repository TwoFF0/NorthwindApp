#nullable disable

namespace Northwind.Services.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public partial class OrderSubtotal
    {
        [Column("OrderID")]
        public int OrderId { get; set; }

        [Column(TypeName = "money")]
        public decimal? Subtotal { get; set; }
    }
}

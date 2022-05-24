﻿#nullable disable

namespace Northwind.Services.Entities
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public partial class ProductsByCategory
    {
        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        public short? UnitsInStock { get; set; }

        public bool Discontinued { get; set; }
    }
}

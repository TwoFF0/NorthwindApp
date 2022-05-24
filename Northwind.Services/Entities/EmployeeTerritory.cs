#nullable disable

namespace Northwind.Services.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class EmployeeTerritory
    {
        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }

        [Key]
        [Column("TerritoryID")]
        [StringLength(20)]
        public string TerritoryId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty("EmployeeTerritories")]
        public virtual EmployeeEntity EmployeeEntity { get; set; }

        [ForeignKey(nameof(TerritoryId))]
        [InverseProperty("EmployeeTerritories")]
        public virtual Territory Territory { get; set; }
    }
}

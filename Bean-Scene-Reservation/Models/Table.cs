using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bean_Scene_Reservation.Models
{
    public class Table
    {
        [Key]
        [StringLength(3, MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]{1,2}\d{1,3}$", ErrorMessage = "Must follow the pattern XX000, e.g. M1, AB001")]
        [DisplayName("Table Number")]
        public string TableNumber { get; set; }

        // Foreign Key to Area
        [Required]
        [DisplayName("Area")]
        public int AreaId { get; set; }

        // Associations
        [DeleteBehavior(DeleteBehavior.Restrict)] // Or NoAction (same for SQL Server)
        public Area? Area { get; set; }

        // Constructors 
        public Table()
        {
            TableNumber = string.Empty;
        }

        public Table(string tableNumber, int areaId)
        {
            TableNumber = tableNumber;
            AreaId = areaId;
        }
    }
}

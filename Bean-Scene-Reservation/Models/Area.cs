using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bean_Scene_Reservation.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Area
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Must have a name!")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Must be between 2-30 characters!")]
        public string Name { get; set; }


        // Navigation properties back to the list of associated tables
        public ICollection<Table>? Tables { get; set; } = null;

        // Constructors 
        public Area()
        {
            Name = string.Empty;
        }

        public Area(string name)
        {
            Name = name;
        }
    }
}

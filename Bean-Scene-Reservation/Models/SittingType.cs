using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bean_Scene_Reservation.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class SittingType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Must be between 2-30 characters!")]
        public string Name { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Scene_Reservation.Models
{
    // Tell ASP.NET Identity to use our custom ApplicationUser implementation
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2-50 characters!")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2-50 characters!")]
        public string LastName { get; set; } = null!;

        [NotMapped]
        public string FullName { get => $"{FirstName} {LastName}"; }
    }
}

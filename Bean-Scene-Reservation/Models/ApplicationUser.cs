using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Scene_Reservation.Models
{
    // Tell ASP.NET Identity to use our custom ApplicationUser implementation
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2-50 characters!")]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2-50 characters!")]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;

        [NotMapped]
        [DisplayName("Full Name")]
        public string FullName { get => $"{FirstName} {LastName}"; }
    }
}

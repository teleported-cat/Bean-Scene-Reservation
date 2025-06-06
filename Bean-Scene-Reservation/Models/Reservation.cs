using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Scene_Reservation.Models
{
    public class Reservation
    {
        [Key]
        [DisplayName("Reservation No.")]
        public int Id { get; set; }

        // TODO: Add ApplicationUser foreign key!
        //[StringLength(450)]
        //[DisplayName("User")]
        //public string? UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}")]
        public DateOnly Date { get; set; }

        [Required]
        [DisplayName("Sitting Type")]
        public int SittingTypeId { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:h:mm tt}")]
        [DisplayName("Start Time")]
        public TimeOnly StartTimeId { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:h:mm tt}")]
        [DisplayName("End Time")]
        public TimeOnly EndTimeId { get; set; }

        [Required]
        [DisplayName("Table Area")]
        public int AreaId { get; set; }

        [Required]
        [DisplayName("No. of Guests")]
        [Range(1, 500)]
        public ushort NumberOfGuests { get; set; } = 1;

        [Required]
        [DisplayName("First name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2-50 characters")]
        public string FirstName { get; set; } = null!;

        [Required]
        [DisplayName("Last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2-50 characters")]
        public string LastName { get; set; } = null!;

        [NotMapped]
        [DisplayName("Full name")]
        public string FullName { get => $"{FirstName.Trim()} {LastName.Trim()}"; }

        [StringLength(256)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        public string? Note { get; set; }

        public ReservationStatus Status { get; set; }

        public enum ReservationStatus
        {
            Pending = 0,
            Confirmed = 1,
            InProgress = 2,
            Completed = 3,
            Cancelled = 4,
        }

        // Associations (Navigation properties)

        [ForeignKey("Date, SittingTypeId")]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public SittingType? SittingType { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        [DisplayName("Start Time")]
        public Timeslot? StartTime { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        [DisplayName("End Time")]
        public Timeslot? EndTime { get; set; }

        [DisplayName("Table Area")]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Area? Area { get; set; }

        //[DeleteBehavior(DeleteBehavior.SetNull)]
        //public ApplicationUser? User { get; set; }

        // Define a many-to-many relationship between Reservation and Table (check the Table model)
        [DisplayName("Assigned Tables")]
        public ICollection<Table> Table { get; } = [];
    }
}

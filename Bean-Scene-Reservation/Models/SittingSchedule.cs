using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bean_Scene_Reservation.Models
{
    public class SittingSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Must be between 2-30 characters!")]
        public string Name { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}")]
        [DisplayName("Start Date")]
        public DateOnly StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}")]
        [DisplayName("End Date")]
        public DateOnly EndDate { get; set; }

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
        [DisplayName("Sitting Type")]
        public int SittingTypeId { get; set; }

        [Required]
        [DisplayName("")]
        public bool ForMonday { get; set; }

        [Required]
        public bool ForTuesday { get; set; }

        [Required]
        public bool ForWednesday { get; set; }

        [Required]
        public bool ForThursday { get; set; }

        [Required]
        public bool ForFriday { get; set; }

        [Required]
        public bool ForSaturday { get; set; }

        [Required]
        public bool ForSunday { get; set; }

        // Associations

        [DisplayName("Start Time")]
        [DeleteBehavior(DeleteBehavior.Restrict)] // Or NoAction (same for SQL Server)
        public Timeslot? StartTime { get; set; }

        [DisplayName("End Time")]
        [DeleteBehavior(DeleteBehavior.Restrict)] // Or NoAction (same for SQL Server)
        public Timeslot? EndTime { get; set; }

        [DisplayName("Sitting Type")]
        [DeleteBehavior(DeleteBehavior.Restrict)] // Or NoAction (same for SQL Server)
        public SittingType? SittingType { get; set; }
    }
}

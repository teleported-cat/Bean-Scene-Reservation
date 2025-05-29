using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bean_Scene_Reservation.Models
{
    [PrimaryKey(nameof(Date), nameof(SittingTypeId))]
    public class Sitting
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:h MMM yyyy}")]
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
        public SittingStatus Status { get; set; }

        public enum SittingStatus
        {
            Available = 1,
            Unavailable = 2,
        }

        [Required]
        public int Capacity { get; set; }

        // TODO: Add nullable foreign key for SittingSchedule

        // Associations

        [DisplayName("Session Type")]
        [DeleteBehavior(DeleteBehavior.Restrict)] // Or NoAction (same for SQL Server)
        public SittingType? SittingType { get; set; }

        [DisplayName("Start Time")]
        [DeleteBehavior(DeleteBehavior.Restrict)] // Or NoAction (same for SQL Server)
        public Timeslot? StartTime { get; set; }

        [DisplayName("End Time")]
        [DeleteBehavior(DeleteBehavior.Restrict)] // Or NoAction (same for SQL Server)
        public Timeslot? EndTime { get; set; }
    }
}

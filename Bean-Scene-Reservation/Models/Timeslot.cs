using System.ComponentModel.DataAnnotations;

namespace Bean_Scene_Reservation.Models
{
    public class Timeslot
    {
        [Key]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public TimeOnly Time { get; set; }
    }
}

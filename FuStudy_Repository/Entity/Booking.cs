using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{

    [Table("Booking")]
    public class Booking
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }

        public long MentorId { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime EndTime { get; set; }

        [Required]
        public string Status { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [ForeignKey("MentorId")]
        public required Mentor Mentor { get; set; }
    }
}

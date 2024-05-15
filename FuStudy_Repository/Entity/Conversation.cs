using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Conversation")]
    public class Conversation
    {
        [Key]
        public long Id { get; set; }

        public long User1Id { get; set; }

        public long User2Id { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime EndTime { get; set; }

        [Required]
        public string LastMessage { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public bool IsClose { get; set; }

        [ForeignKey("User1Id")]
        public required User User1 { get; set; }

        [ForeignKey("User2Id")]
        public required User User2 { get; set; }
    }
}

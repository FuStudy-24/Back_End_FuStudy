using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("MentorMajor")]
    public class MentorMajor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long MentorId { get; set; }

        public long MajorId { get; set; }

        [ForeignKey("MentorId")]
        public required Mentor Mentor { get; set; }

        [ForeignKey("MajorId")]
        public required Major Major { get; set; }
    }
}

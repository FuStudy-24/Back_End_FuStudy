using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("StudentSubcription")]
    public class StudentSubcription
    {
        public long StudentId { get; set; }

        public long SubcriptionId { get; set; }

        [Required]
        public int LimitQuestion { get; set; }

        [Required]
        public int CurrentQuestion { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [ForeignKey("StudentId")]
        public required Student Student { get; set; }

        [ForeignKey("SubcriptionId")]
        public required Subcription Subcription { get; set; }
    }
}

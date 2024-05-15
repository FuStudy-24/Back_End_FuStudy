using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Subcription")]
    public class Subcription
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string SubcriptionName { get; set; }

        [Required]
        public double SubcriptionPrice { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}

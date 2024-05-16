using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Question")]
    public class Question
    {
        [Key]
        public long Id { get; set; }

        public long StudentId { get; set; }

        public long CategoryId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public bool Status { get; set; }

        [ForeignKey("StudentId")]
        public required Student Student { get; set; }

        [ForeignKey("CategoryId")]
        public required Category Category { get; set; }

    }
}

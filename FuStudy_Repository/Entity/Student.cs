using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}

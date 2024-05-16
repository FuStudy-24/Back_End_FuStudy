using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Mentor")]
    public class Mentor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long UserId { get; set; }

        public long MajorId {  get; set; }

        [Required]
        public string AcademicLevel { get; set; }

        [Required]
        public string WorkPlace { get; set; }

        [Required]
        public string OnlineStatus { get; set; }

        [Required]
        public string Skill { get; set; }

        public string Video { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [ForeignKey("MajorId")]
        public required Major Major { get; set; }
    }
}

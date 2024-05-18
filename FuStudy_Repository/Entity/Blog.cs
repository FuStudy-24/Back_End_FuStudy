using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Blog")]
    public class Blog
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Image { get; set; }

        public DateTime CreateDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}

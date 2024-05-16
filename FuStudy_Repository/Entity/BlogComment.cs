using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("BlogComment")]
    public class BlogComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long UserId { get; set; }

        public long BlogId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        public bool Status { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [ForeignKey("BlogId")]
        public required Blog Blog { get; set; }
    }
}

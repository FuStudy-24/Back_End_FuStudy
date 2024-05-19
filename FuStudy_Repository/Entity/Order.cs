using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long SubcriptionId { get; set; }

        public long StudentId { get; set; }

        [Required]
        public string PaymentCode { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        public double Money { get; set; }

        [Required]
        public bool Status { get; set; }

        [ForeignKey("SubcriptionId")]
        public virtual Subcription Subcription { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}

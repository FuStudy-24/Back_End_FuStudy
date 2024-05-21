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

        public long TransactionId { get; set; }

        [Required]
        public string PaymentCode { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        public double Money { get; set; }

        [Required]
        public bool Status { get; set; }

        [ForeignKey("TransactionId")]
        public virtual Transaction Transaction { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public long Id { get; set; }

        public long WalletId { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public double Ammount { get; set; }

        public DateTime CreateTime { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("WalletId")]
        public required Wallet Wallet { get; set; }
    }
}

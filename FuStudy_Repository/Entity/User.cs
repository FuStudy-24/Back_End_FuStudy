using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("User")]
    public class User
    {
        [Key]
        public long Id { get; set; }

        public long RoleId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string IdentityCard { get; set; }

        [Required]
        public DateOnly Dob { get; set; }

        [Required]
        public string Phone { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        public bool Status { get; set; }

        [ForeignKey("RoleId")]
        public required Role Role { get; set; }
    }
}

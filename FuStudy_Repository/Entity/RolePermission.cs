using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("RolePermission")]
    public class RolePermission
    {
        [Key]
        public long Id { get; set; }

        public long RoleId { get; set; }

        public long PermissionId { get; set; }

        [ForeignKey("RoleId")]
        public required Role Role { get; set; }

        [ForeignKey("PermissionId")]
        public required Permission Permission { get; set; }

    }
}

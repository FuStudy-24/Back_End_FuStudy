using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Response
{
    public class RolePermissionResponse
    {
        public long Id { get; set; }

        public long RoleId { get; set; }

        public long PermissionId { get; set; }
    }
}

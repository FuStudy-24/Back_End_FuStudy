using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Request
{
    public class BlogLikeRequest
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long BlogId { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}

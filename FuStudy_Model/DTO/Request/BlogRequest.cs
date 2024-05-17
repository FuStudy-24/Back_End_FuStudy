using FuStudy_Repository.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Request
{
    public class BlogRequest
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z0-9@#$&()_]*$")]
        public string BlogContent { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
    }
}

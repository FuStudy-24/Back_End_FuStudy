using FuStudy_Repository.Entity;
using Microsoft.AspNetCore.Http;
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
        public string BlogContent { get; set; }
        //[Required]
        //public string Image { get; set; }
        //[Required]
        //public DateTime CreateDate { get; set; }
        public IFormFile? FormFile { get; set; } = null;
    }
}

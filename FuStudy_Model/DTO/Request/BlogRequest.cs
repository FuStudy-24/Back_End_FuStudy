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
        public string BlogContent { get; set; }
        public IFormFile? FormFile { get; set; } = null;
    }
}

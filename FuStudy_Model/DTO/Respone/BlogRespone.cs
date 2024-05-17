using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Respone
{
    public class BlogRespone
    {
        public long UserId { get; set; }
        public string BlogContent { get; set; }
        public string Image { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

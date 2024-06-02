using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Request
{
    public class CommentImageRequest
    {
        public long BlogCommentId { get; set; }
        public string Image { get; set; }
    }
}

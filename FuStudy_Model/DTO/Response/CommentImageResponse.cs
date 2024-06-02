using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Response
{
    public class CommentImageResponse
    {
        public long Id { get; set; }
        public long BlogCommentId { get; set; }
        public string Image { get; set; }
    }
}

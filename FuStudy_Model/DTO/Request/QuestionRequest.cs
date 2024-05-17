using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Request
{
    public class QuestionRequest
    {
        public long StudentId { get; set; }

        public long CategoryId { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

    }
}

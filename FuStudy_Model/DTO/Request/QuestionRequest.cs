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
        private long StudentId { get; set; }

        private long CategoryId { get; set; }

        private string Content { get; set; }

        private DateTime CreateDate { get; set; }

    }
}

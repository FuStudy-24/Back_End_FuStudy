using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Respone
{
    public class QuestionResponse
    {
        private long StudentId { get; set; }

        private long CategoryId { get; set; }

        private string Content { get; set; }

        private DateTime CreateDate { get; set; }

        private DateTime ModifiedDate { get; set; }
    }
}

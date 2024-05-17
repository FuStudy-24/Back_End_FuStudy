using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Respone
{
    public class QuestionResponse
    {
        public long Id;
        
        public long StudentId { get; set; }

        public long CategoryId { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}

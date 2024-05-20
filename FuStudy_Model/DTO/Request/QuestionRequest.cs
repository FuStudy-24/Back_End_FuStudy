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
        [Required(ErrorMessage = "Student is required!")]
        public long StudentId { get; set; }
        
        [Required(ErrorMessage = "Category is required!")]
        public long CategoryId { get; set; }
        
        [Required(ErrorMessage = "Content is required!")]
        public string Content { get; set; }
        
        public string Image { get; set; }

        [Required(ErrorMessage = "CreateDate is required!")]
        public DateTime CreateDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Respone
{
    public class SubcriptionResponse
    {
        public long id { get; set; }
        public string subcriptionName { get; set; }
        public double subcriptionPrice { get; set; }
        public bool status { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Request
{
    public class CreateStudentSubcriptionRequest
    {
        public long StudentId { get; set; }

        public long SubcriptionId { get; set; }
    }
}
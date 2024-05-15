﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}

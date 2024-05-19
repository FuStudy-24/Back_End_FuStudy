﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("Attachment")]
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ConversationMessageId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        public long FileSize { get; set; }

        [Required]
        public string FilePath { get; set; }
        
        public DateTime CreateAt { get; set; }

        [ForeignKey("ConversationMessageId")]
        public virtual ConversationMessage ConversationMessage { get; set; }
    }
}

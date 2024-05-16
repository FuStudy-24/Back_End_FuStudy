﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Repository.Entity
{
    [Table("MessageReaction")]
    public class MessageReaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long UserId { get; set; }

        public long ConversationMessageId { get; set; }

        [Required]
        public string ReactionType { get; set; }

        public DateTime CreateAt { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [ForeignKey("ConversationMessageId")]
        public required ConversationMessage ConversationMessage { get; set; }
    }
}

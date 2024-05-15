using System;
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
        public long id { get; set; }

        public long userId { get; set; }

        public long conversationMessageId { get; set; }

        [Required]
        public string reactionType { get; set; }

        public DateTime createAt { get; set; }

        [ForeignKey("userId")]
        public User user { get; set; }

        [ForeignKey("conversationMessageId")]
        public ConversationMessage conversationMessage { get; set; }
    }
}

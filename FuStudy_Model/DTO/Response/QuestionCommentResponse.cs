using System;

namespace FuStudy_Model.DTO.Response;

public class QuestionCommentResponse
{
    public long UserId { get; set; }

    public long QuestionId { get; set; }
    
    public string Content { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public bool Status { get; set; }
}
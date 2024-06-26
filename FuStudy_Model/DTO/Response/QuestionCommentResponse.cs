using System;

using FuStudy_Repository.Entity;

namespace FuStudy_Model.DTO.Response;

public class QuestionCommentResponse
{
    
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public string Username { get; set; }


    public long QuestionId { get; set; }

    public string Content { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public bool Status { get; set; }

    public bool IsMentor { get; set; }
    public QuestionResponse QuestionResponse { get; set; } 

}
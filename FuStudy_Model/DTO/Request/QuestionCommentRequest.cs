namespace FuStudy_Model.DTO.Request;

public class QuestionCommentRequest
{
    public long UserId { get; set; }

    public long QuestionId { get; set; }
    
    public string Content { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime ModifiedDate { get; set; }
    
    public bool Status { get; set; }
}
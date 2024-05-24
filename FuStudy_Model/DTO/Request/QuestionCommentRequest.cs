namespace FuStudy_Model.DTO.Request;

public class QuestionCommentRequest
{
    public long UserId { get; set; }

    public long QuestionId { get; set; }
    
    public string Content { get; set; }
    
}
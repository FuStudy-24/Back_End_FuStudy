using System.ComponentModel.DataAnnotations;

namespace FuStudy_Model.DTO.Request;

public class QuestionRatingRequest
{
    [Required (ErrorMessage = "User Id is required!")]
    public long UserId { get; set; }
    
    [Required (ErrorMessage = "Question Id is required!")]
    public long QuestionId { get; set; }
    
}
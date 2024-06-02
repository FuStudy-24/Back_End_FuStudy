using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;

namespace FuStudy_Service.Interface;

public interface IQuestionRatingService
{
    Task<IEnumerable<QuestionRatingResponse>> GetAllQuestionsRatings();
    Task<QuestionRatingResponse> GetQuestionRatingById(long id);
    Task<QuestionRatingResponse> LikeQuestion(QuestionRatingRequest questionRatingRequest);
    Task UnlikeQuestion(QuestionRatingRequest questionRatingRequest);
    Task<IEnumerable<QuestionRatingResponse>> GetAllQuestionRatingByQuestionId(long questionId);
}
using System.Net;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;

namespace FuStudy_Service.Interface;

public interface IQuestionRatingService
{
    Task<IEnumerable<QuestionRatingResponse>> GetAllQuestionsRatings();
    Task<QuestionRatingResponse> GetQuestionCommentById(long id);
    Task<QuestionRatingResponse> CreateQuestionRating(QuestionRatingRequest questionRatingRequest);
    Task<QuestionRatingResponse> UpdateQuestionRating(QuestionRequest questionRequest, long questionId);
    Task DeleteQuestionRating(long questionId);
}
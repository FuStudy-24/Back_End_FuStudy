using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Service.Interface;

namespace FuStudy_Service.Service;

public class QuestionRatingService : IQuestionRatingService
{
    public Task<QuestionRatingResponse> GetAllQuestionsRatings()
    {
        throw new NotImplementedException();
    }

    public Task<QuestionRatingResponse> GetQuestionCommentById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<QuestionRatingResponse> CreateQuestionRating(QuestionRatingRequest questionRatingRequest)
    {
        throw new NotImplementedException();
    }

    public Task<QuestionRatingResponse> UpdateQuestionRating(QuestionRequest questionRequest, long questionId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteQuestionRating(long questionId)
    {
        throw new NotImplementedException();
    }
}
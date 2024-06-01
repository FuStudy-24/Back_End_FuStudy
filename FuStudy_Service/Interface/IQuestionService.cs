using System.Collections.Generic;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using System.Threading.Tasks;

namespace FuStudy_Service.Interface
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync();

        Task<QuestionResponse> GetQuestionByIdAsync(long id);

        Task<QuestionResponse> CreateQuestionAsync(QuestionRequest questionRequest);
        
        Task<QuestionResponse> UpdateQuestionAsync(QuestionRequest questionRequest, long questionId);
        
        Task<bool> DeleteQuestionAsync(long questionId);

        Task<bool> IsExistByQuestionId(long questionId);

    }
}

using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Respone;
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

    }
}

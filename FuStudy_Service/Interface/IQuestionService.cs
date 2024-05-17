using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Respone;
using FuStudy_Repository.Entity;


namespace FuStudy_Service.Interface
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> GetAllQuestions();

        Task<QuestionResponse> GetQuestionById(long id);

        Task<QuestionResponse> CreateQuestion(QuestionRequest questionRequest);
    }
}

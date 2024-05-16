using FuStudy_Repository.Entity;


namespace FuStudy_Service.Interface
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> GetAllQuestions();
    }
}

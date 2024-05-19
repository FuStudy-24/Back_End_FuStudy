using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.ObjectModel;

namespace FuStudy_Service.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync()
        {
            var questions = _unitOfWork.QuestionRepository.Get();
            return _mapper.Map<IEnumerable<QuestionResponse>>(questions);
        }

        public async Task<QuestionResponse> GetQuestionByIdAsync(long id)
        {
            var question = _unitOfWork.QuestionRepository.GetByID(id);
            return _mapper.Map<QuestionResponse>(question);
        }


        public async Task<QuestionResponse> CreateQuestionAsync(QuestionRequest questionRequest)
        {

            var question = _mapper.Map<Question>(questionRequest);
            _unitOfWork.QuestionRepository.Insert(question);
            _unitOfWork.Save();

            return _mapper.Map<QuestionResponse>(question);
        }

        public async Task<QuestionResponse> UpdateQuestionAsync(QuestionRequest questionRequest, long questionId)
        {
            var question = _unitOfWork.QuestionRepository.GetByID(questionId);

            if (question == null)
            {
                throw new Exception($"Question with ID: {questionId} not found");
            }

            _mapper.Map(questionRequest, question);
            _unitOfWork.QuestionRepository.Update(question);
            _unitOfWork.Save();

            return _mapper.Map<QuestionResponse>(question);

        }

        public async Task<bool> DeleteQuestionAsync(long questionId)
        {
            var deletedQuestion = _unitOfWork.QuestionRepository.GetByID(questionId);

            if (deletedQuestion == null)
            {
                throw new Exception($"Question with ID: {questionId} not found");
            }
            
            _unitOfWork.QuestionRepository.Delete(deletedQuestion);
            _unitOfWork.Save();
            return true;
        }
    }
}

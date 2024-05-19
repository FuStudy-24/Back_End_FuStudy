using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.ObjectModel;
using Tools;

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
            if (_unitOfWork.StudentRepository.GetByID(questionRequest.StudentId) == null)
            {
                throw new CustomException.DataNotFoundException($"Student with this {questionRequest.StudentId} not found!");
            }
            if (_unitOfWork.CategoryRepository.GetByID(questionRequest.CategoryId) == null)
            {
                throw new CustomException.DataNotFoundException($"Category with this {questionRequest.CategoryId} not found!");
            }

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
                throw new CustomException.DataNotFoundException($"Question with ID: {questionId} not found");
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
                throw new CustomException.DataNotFoundException($"Question with ID: {questionId} not found");
            }
            
            _unitOfWork.QuestionRepository.Delete(deletedQuestion);
            _unitOfWork.Save();
            return true;
        }
    }
}

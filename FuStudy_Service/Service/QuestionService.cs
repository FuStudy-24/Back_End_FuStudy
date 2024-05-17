using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Respone;
using FuStudy_Repository;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository.Interface;
using FuStudy_Service.Interface;
using System;
using System.Collections.ObjectModel;

namespace FuStudy_Service.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public QuestionService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionResponse>> GetAllQuestions()
        {
            var questions = _unitOfWork.QuestionRepository.GetAll();
            return _mapper.Map<IEnumerable<QuestionResponse>>(questions);
        }

        public async Task<QuestionResponse> GetQuestionById(long id)
        {
            var question = _unitOfWork.QuestionRepository.GetById(id);
            return _mapper.Map<QuestionResponse>(question);
        }


        public async Task<QuestionResponse> CreateQuestion(QuestionRequest questionRequest)
        {
            throw new NotImplementedException();
        }
    }
}

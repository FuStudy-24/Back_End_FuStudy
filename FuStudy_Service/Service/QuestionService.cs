using AutoMapper;
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

        public Task<IEnumerable<Question>> GetAllQuestions()
        {
            var questions = _unitOfWork.QuestionRepository.Get();
            return Task.FromResult(questions);
        }

    }
}

using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Tools;

namespace FuStudy_Service.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync(QueryObject queryObject)
        {
            //check if QueryObject search is not null
            Expression<Func<Question, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(queryObject.Search))
            {
                filter = question => question.Content.Contains(queryObject.Search);
            }

            var questions = _unitOfWork.QuestionRepository.Get(
                filter: filter,
                pageIndex: queryObject.PageIndex,
                pageSize: queryObject.PageSize);
            if (questions.IsNullOrEmpty())
            {
                throw new CustomException.DataNotFoundException("The question list is empty!");
            }

            return _mapper.Map<IEnumerable<QuestionResponse>>(questions);
        }

        public async Task<QuestionResponse> GetQuestionByIdAsync(long id)
        {
            var question = _unitOfWork.QuestionRepository.GetByID(id);
            return _mapper.Map<QuestionResponse>(question);
        }


        public async Task<QuestionResponse> CreateQuestionWithSubscription(QuestionRequest questionRequest)
        {
            var userId = long.Parse(Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext));
            var studentId = GetStudentIdByUserId(userId);
            var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryName == questionRequest.CategoryName).FirstOrDefault();
            if (_unitOfWork.StudentRepository.GetByID(studentId) == null)
            {
                throw new CustomException.DataNotFoundException($"Student with ID: {studentId} not found!");
            }
            if (_unitOfWork.CategoryRepository.GetByID(category.Id) == null)
            {
                throw new CustomException.DataNotFoundException($"Category with ID: {category.Id} not found!");
            }
            //check if student having subscription
            
            var studentSubscription = _unitOfWork.StudentSubcriptionRepository
                .Get(s => s.StudentId == studentId && s.Status == true).FirstOrDefault();

            if (studentSubscription == null)
            {
                throw new CustomException.InvalidDataException("This Student does not have subscription");
            }

            var subscription = _unitOfWork.SubcriptionRepository.GetByID(studentSubscription.SubcriptionId);

            //check if current question equal to limit subscription
            if (studentSubscription.CurrentQuestion == subscription.LimitQuestion)
            {
                throw new CustomException.InvalidDataException("The student's current question have reach limit!");
            }
            
            studentSubscription.CurrentQuestion++;
 
            var questionWithSubscription = _mapper.Map<Question>(questionRequest);
            questionWithSubscription.CreateDate = DateTime.Now;
            questionWithSubscription.TotalRating = 0;
            questionWithSubscription.StudentId = studentId;
            questionWithSubscription.CategoryId = category.Id;
            _unitOfWork.QuestionRepository.Insert(questionWithSubscription);
            _unitOfWork.Save();            
            return _mapper.Map<QuestionResponse>(questionWithSubscription);

        }
        

        public async Task<QuestionResponse> CreateQuestionByCoin(QuestionRequest questionRequest)
        {
            var userId = long.Parse(Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext));
            var studentId = GetStudentIdByUserId(userId);
            var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryName == questionRequest.CategoryName).FirstOrDefault();

            if (_unitOfWork.StudentRepository.GetByID(studentId) == null)
            {
                throw new CustomException.DataNotFoundException($"Student with ID: {studentId} not found!");
            }
            if (_unitOfWork.CategoryRepository.GetByID(category.Id) == null)
            {
                throw new CustomException.DataNotFoundException($"Category with ID: {category.Id} not found!");
            }
            
            var student = _unitOfWork.StudentRepository.GetByID(studentId);
            //If having coin
            //20 FuCoin per question
            var userWallet = _unitOfWork.WalletRepository.Get(wallet => wallet.UserId == student.UserId).FirstOrDefault();
            if (userWallet.Balance < 20)
            {
                throw new CustomException.InvalidDataException("You dont have enough FuCoin!");
            }

            userWallet.Balance -= 20;
            //save to database
            var questionWithCoin = _mapper.Map<Question>(questionRequest);
            questionWithCoin.CategoryId = category.Id;
            questionWithCoin.StudentId = studentId;
            questionWithCoin.CreateDate = DateTime.Now;
            questionWithCoin.TotalRating = 0;
            _unitOfWork.QuestionRepository.Insert(questionWithCoin);
            _unitOfWork.Save();
            
            return _mapper.Map<QuestionResponse>(questionWithCoin);

        }


        public async Task<QuestionResponse> UpdateQuestionAsync(QuestionRequest questionRequest, long questionId)
        {
            var question = _unitOfWork.QuestionRepository.GetByID(questionId);

            if (question == null)
            {
                throw new CustomException.DataNotFoundException($"Question with ID: {questionId} not found");
            }

            _mapper.Map(questionRequest, question);
            question.ModifiedDate = DateTime.Now;
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

        public async Task<bool> IsExistByQuestionId(long questionId)
        {
            var isExist = await _unitOfWork.QuestionRepository.ExistsAsync(question => question.Id == questionId);
            return isExist;
        }

        public long GetStudentIdByUserId(long userId)
        {
            var student = _unitOfWork.StudentRepository.Get(s => s.UserId == userId).FirstOrDefault();
            if (student == null)
            {
                throw new CustomException.DataNotFoundException("This user is not a student <3");
            }
            return student.Id;
        }
    }
}
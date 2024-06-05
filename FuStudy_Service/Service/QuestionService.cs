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
using Microsoft.IdentityModel.Tokens;
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
            if (_unitOfWork.StudentRepository.GetByID(questionRequest.StudentId) == null)
            {
                throw new CustomException.DataNotFoundException($"Student with ID: {questionRequest.StudentId} not found!");
            }
            if (_unitOfWork.CategoryRepository.GetByID(questionRequest.CategoryId) == null)
            {
                throw new CustomException.DataNotFoundException($"Category with ID: {questionRequest.CategoryId} not found!");
            }
            //check if student having subscription
            var studentSubscription = await _unitOfWork.StudentSubcriptionRepository.GetByFilterAsync(s => s.StudentId == questionRequest.StudentId && s.Status == true);
            var selectedStudentSubscription = studentSubscription.FirstOrDefault();
            var selectedSubscription =
                _unitOfWork.SubcriptionRepository.GetByID(selectedStudentSubscription.SubcriptionId);

            if (!(selectedStudentSubscription == null))
            {

                //check if current question equal to limit subscription
                if (selectedStudentSubscription.CurrentQuestion == selectedSubscription.LimitQuestion)
                {
                    throw new CustomException.InvalidDataException("The student's current question have reach limit!");
                }
                selectedStudentSubscription.CurrentQuestion++;
            }
            else
            {
                throw new CustomException.InvalidDataException("This User does not have valid subscription!");
            }
            
            var questionWithCoin = _mapper.Map<Question>(questionRequest);
            questionWithCoin.CreateDate = DateTime.Now;
            questionWithCoin.TotalRating = 0;
            _unitOfWork.QuestionRepository.Insert(questionWithCoin);
            _unitOfWork.Save();

            return _mapper.Map<QuestionResponse>(questionWithCoin);
        }

        public async Task<QuestionResponse> CreateQuestionByCoin(QuestionRequest questionRequest)
        {
            if (_unitOfWork.StudentRepository.GetByID(questionRequest.StudentId) == null)
            {
                throw new CustomException.DataNotFoundException($"Student with ID: {questionRequest.StudentId} not found!");
            }
            if (_unitOfWork.CategoryRepository.GetByID(questionRequest.CategoryId) == null)
            {
                throw new CustomException.DataNotFoundException($"Category with ID: {questionRequest.CategoryId} not found!");
            }
            
            var student = _unitOfWork.StudentRepository.GetByID(questionRequest.StudentId);
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
    }
}
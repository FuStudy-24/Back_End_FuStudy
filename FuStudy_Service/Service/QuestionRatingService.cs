using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using Tools;

namespace FuStudy_Service.Service;

public class QuestionRatingService : IQuestionRatingService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QuestionRatingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IEnumerable<QuestionRatingResponse>> GetAllQuestionsRatings()
    {
        var questionRatings = _unitOfWork.QuestionRatingRepository.Get();
        return _mapper.Map<IEnumerable<QuestionRatingResponse>>(questionRatings);
    }

    public async Task<QuestionRatingResponse> GetQuestionCommentById(long id)
    {
        var question = _unitOfWork.QuestionRatingRepository.GetByID(id);
        return _mapper.Map<QuestionRatingResponse>(question);
    }

    public async Task<QuestionRatingResponse> CreateQuestionRating(QuestionRatingRequest questionRatingRequest)
    {
        if (_unitOfWork.UserRepository.GetByID(questionRatingRequest.UserId) == null)
        {
            throw new CustomException.DataNotFoundException($"Student with this {questionRatingRequest.UserId} not found!");
        }
        if (_unitOfWork.QuestionRepository.GetByID(questionRatingRequest.QuestionId) == null)
        {
            throw new CustomException.DataNotFoundException($"Category with this {questionRatingRequest.QuestionId} not found!");
        }

        var question = _mapper.Map<QuestionRating>(questionRatingRequest);
        _unitOfWork.QuestionRatingRepository.Insert(question);
        _unitOfWork.Save();

        return _mapper.Map<QuestionRatingResponse>(question);
    }

    public async Task<QuestionRatingResponse> UpdateQuestionRating(QuestionRequest questionRequest, long questionId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteQuestionRating(long questionId)
    {
        throw new NotImplementedException();
    }
}
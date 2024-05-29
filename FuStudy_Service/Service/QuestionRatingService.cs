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

    public async Task<QuestionRatingResponse> GetQuestionRatingById(long id)
    {
        var question = _unitOfWork.QuestionRatingRepository.GetByID(id);
        return _mapper.Map<QuestionRatingResponse>(question);
    }

    public async Task<QuestionRatingResponse> LikeQuestion(QuestionRatingRequest questionRatingRequest)
    {
        if (_unitOfWork.UserRepository.GetByID(questionRatingRequest.UserId) == null)
        {
            throw new CustomException.DataNotFoundException($"Student with this {questionRatingRequest.UserId} not found!");
        }
        if (_unitOfWork.QuestionRepository.GetByID(questionRatingRequest.QuestionId) == null)
        {
            throw new CustomException.DataNotFoundException($"Category with this {questionRatingRequest.QuestionId} not found!");
        }

        //create question rating
        var questionRating = _mapper.Map<QuestionRating>(questionRatingRequest);
        //check if rating is exist
        bool isExist = await RatingExists(questionRating.QuestionId, questionRatingRequest.UserId);
        if (isExist)
        {
            throw new CustomException.InvalidDataException("500", "This Rating is duplicated!");
        }
        questionRating.Status = true;
        _unitOfWork.QuestionRatingRepository.Insert(questionRating);
        var question = _unitOfWork.QuestionRepository.GetByID(questionRating.QuestionId);
        //add total like into question
        question.TotalRating++;
        _unitOfWork.QuestionRepository.Update(question);
        _unitOfWork.Save();

        return _mapper.Map<QuestionRatingResponse>(questionRating);
    }

    public async Task UnlikeQuestion(QuestionRatingRequest questionRatingRequest)
    {
        if (_unitOfWork.UserRepository.GetByID(questionRatingRequest.UserId) == null)
        {
            throw new CustomException.DataNotFoundException($"Student with this {questionRatingRequest.UserId} not found!");
        }
        if (_unitOfWork.QuestionRepository.GetByID(questionRatingRequest.QuestionId) == null)
        {
            throw new CustomException.DataNotFoundException($"Category with this {questionRatingRequest.QuestionId} not found!");
        }

        var questionRating = _mapper.Map<QuestionRating>(questionRatingRequest);
        
        //check if rating is exist
        bool isExist = await RatingExists(questionRating.QuestionId, questionRatingRequest.UserId);
        if (!isExist)
        {
            throw new CustomException.DataNotFoundException($"This QuestionRating not found!");

        }
        //delete question rating
        _unitOfWork.QuestionRatingRepository.Delete(questionRating);
        
        //subtract question total rating
        var question = _unitOfWork.QuestionRepository.GetByID(questionRatingRequest.QuestionId);
        question.TotalRating--;
        _unitOfWork.QuestionRepository.Update(question);
        _unitOfWork.Save();

    }
    
    public async Task<bool> RatingExists(long questionId, long userId)
    {
        return await _unitOfWork.QuestionRatingRepository.ExistsAsync(r => 
            r.QuestionId == questionId && r.UserId == userId);
    }
}
﻿using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;

namespace FuStudy_Service.Interface;

public interface IQuestionCommentService
{
    Task<IEnumerable<QuestionCommentResponse>> GetAllQuestionComments();
    
    Task<QuestionCommentResponse> GetQuestionCommentById(long id);

    Task<QuestionCommentResponse> CreateQuestionComment(QuestionCommentRequest questionCommentRequest);
        
    Task<QuestionCommentResponse> UpdateQuestionComment(QuestionCommentRequest questionRequest, long questionCommentId);
        
    Task<bool> DeleteQuestionComment(long questionId);
}
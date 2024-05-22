using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using Tools;

namespace FuStudy_Service.Service;

public class QuestionCommentService : IQuestionCommentService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QuestionCommentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<QuestionCommentResponse> GetAllQuestionComments()
    {
        var questionComments =  _unitOfWork.QuestionCommentRepository.Get();
        return _mapper.Map<QuestionCommentResponse>(questionComments);

    }

    public async Task<QuestionCommentResponse> GetQuestionCommentById(long id)
    {
        var questionComments = _unitOfWork.QuestionCommentRepository.GetByID(id);
        return _mapper.Map<QuestionCommentResponse>(questionComments);
    }

    public async Task<QuestionCommentResponse> CreateQuestionComment(QuestionCommentRequest questionCommentRequest)
    {
        if (_unitOfWork.QuestionRepository.GetByID(questionCommentRequest.QuestionId) == null)
        {
            throw new CustomException.DataNotFoundException($"Question with this {questionCommentRequest.QuestionId} not found!");
        }
        if (_unitOfWork.UserRepository.GetByID(questionCommentRequest.UserId) == null)
        {
            throw new CustomException.DataNotFoundException($"User with this {questionCommentRequest.UserId} not found!");
        }

        var questionComment = _mapper.Map<QuestionComment>(questionCommentRequest);
        _unitOfWork.QuestionCommentRepository.Insert(questionComment);
        _unitOfWork.Save();

        return _mapper.Map<QuestionCommentResponse>(questionComment);
    }

    public async Task<QuestionCommentResponse> UpdateQuestionComment(QuestionCommentRequest questionRequest, long questionId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteQuestionComment(long questionId)
    {
        throw new NotImplementedException();
    }
}
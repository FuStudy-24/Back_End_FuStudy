using CoreApiResponse;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FuStudy_API.Controllers.Question;


[Route("api/[controller]")]
[ApiController]
public class QuestionRatingController : BaseController
{
    private readonly IQuestionService _questionService;
    
    public QuestionRatingController (IQuestionService questionService)
    {
        this._questionService = questionService;
    } 
    
}
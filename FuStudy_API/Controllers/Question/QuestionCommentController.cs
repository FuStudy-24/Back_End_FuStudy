using CoreApiResponse;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace FuStudy_API.Controllers.Question;


[Route("api/[controller]")]
[ApiController]
public class QuestionCommentController : BaseController
{
    private readonly IQuestionCommentService _questionCommentService;
    
    public QuestionCommentController (IQuestionCommentService questionCommentServiceService)
    {
        this._questionCommentService = questionCommentServiceService;
    } 
    
    
    [HttpGet("GetAllQuestionComments")]
    public async Task<IActionResult> GetAllQuestionComments()
    {
        var questionsComments = await _questionCommentService.GetAllQuestionComments();
        return CustomResult("Data Loaded!", questionsComments);
    }
    
    


}
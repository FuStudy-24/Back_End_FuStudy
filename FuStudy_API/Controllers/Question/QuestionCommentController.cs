using System;
using System.Net;
using System.Threading.Tasks;
using CoreApiResponse;
using FuStudy_Model.DTO.Request;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Tools;

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
        try
        {
            var questionsComments = await _questionCommentService.GetAllQuestionComments();
            return CustomResult("Data Loaded!", questionsComments);
        }
        catch (CustomException.DataNotFoundException e)
        {
            return CustomResult(e.Message, HttpStatusCode.NotFound);

        }
        catch (Exception exception)
        {
            return CustomResult(exception.Message, HttpStatusCode.InternalServerError);
        }
        
    }
    
      [HttpGet("GetQuestionCommentById/{id}")]
        public async Task<IActionResult> GetQuestionById(long id)
        {
            var question = await _questionCommentService.GetQuestionCommentById(id);

            if (question == null)
            {
                return CustomResult("Question not found", HttpStatusCode.NotFound);
            }

            return CustomResult("Data loaded!", question);

        }

    [HttpPost("CreateQuestionComment")]
        public async Task<IActionResult> CreateQuestionComment([FromBody] QuestionCommentRequest questionCommentRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return CustomResult(ModelState, HttpStatusCode.BadRequest);
                }


                var createdQuestionComment = await _questionCommentService.CreateQuestionComment(questionCommentRequest);



                return CustomResult("Created successfully", createdQuestionComment);
            }
            catch (CustomException.DataNotFoundException e)
            {
                return CustomResult(e.Message, HttpStatusCode.NotFound);

            }
            catch (Exception exception)
            {
                return CustomResult(exception.Message, HttpStatusCode.InternalServerError);
            }
                
            
        }

        [HttpPost("UpdateQuestionComment/{questionCommentId}")]
        public async Task<IActionResult> UpdateQuestionComment(long questionCommentId, [FromBody] QuestionCommentRequest questionRequest)
        {
            if (!ModelState.IsValid)
            {
                return CustomResult(ModelState, HttpStatusCode.BadRequest);
            }

            try
            {
                var updateQuestionComment = await _questionCommentService.UpdateQuestionComment(questionRequest, questionCommentId);
                return CustomResult("Update successfully", updateQuestionComment);
            }
            catch (CustomException.DataNotFoundException e)
            {
                return CustomResult(e.Message, HttpStatusCode.NotFound);

            }
            catch (Exception exception)
            {
                return CustomResult(exception.Message, HttpStatusCode.InternalServerError);
            }

            
        }

        [HttpDelete("DeleteQuestionComment/{questionId}")]
        public async Task<IActionResult> DeleteQuestionComment(long questionId)
        {
            try
            {
                await _questionCommentService.DeleteQuestionComment(questionId);
                return CustomResult("Delete question successfully", HttpStatusCode.NoContent);
            }
            catch (CustomException.DataNotFoundException e)
            {
                return CustomResult(e.Message, HttpStatusCode.NotFound);

            }
            catch (Exception exception)
            {
                return CustomResult(exception.Message, HttpStatusCode.InternalServerError);
            }

        }


}
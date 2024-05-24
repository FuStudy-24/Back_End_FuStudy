using System.Net;
using CoreApiResponse;
using FuStudy_Model.DTO.Request;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Tools;

namespace FuStudy_API.Controllers.Question;


[Route("api/[controller]")]
[ApiController]
public class QuestionRatingController : BaseController
{
    private readonly IQuestionRatingService _questionRatingService;
    
    public QuestionRatingController (IQuestionRatingService questionRatingService)
    {
        this._questionRatingService = questionRatingService;
    } 
    
    [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestionRatings()
        {
            var questionRatings = await _questionRatingService.GetAllQuestionsRatings();
            return CustomResult("Data loaded!", questionRatings);
        }


        [HttpGet("GetQuestionById/{id}")]
        public async Task<IActionResult> GetQuestionById(long id)
        {
            var question = await _questionRatingService.GetQuestionCommentById(id);

            if (question == null)
            {
                return CustomResult("Question not found", HttpStatusCode.NotFound);
            }

            return CustomResult("Data loaded!", question);

        }

    [HttpPost("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionRatingRequest questionRatingRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return CustomResult(ModelState, HttpStatusCode.BadRequest);
                }


                var createdQuestion = await _questionRatingService.CreateQuestionRating(questionRatingRequest);



                return CustomResult("Created successfully", createdQuestion);
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

        [HttpPost("UpdateQuestion/{questionId}")]
        public async Task<IActionResult> UpdateQuestion(long questionId, [FromBody] QuestionRequest questionRequest)
        {
            if (!ModelState.IsValid)
            {
                return CustomResult(ModelState, HttpStatusCode.BadRequest);
            }

            try
            {
                var updateQuestion = await _questionRatingService.UpdateQuestionRating(questionRequest, questionId);
                return CustomResult("Update successfully", updateQuestion);
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

        [HttpDelete("DeleteQuestion/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(long questionId)
        {
            try
            {
                await _questionRatingService.DeleteQuestionRating(questionId);
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
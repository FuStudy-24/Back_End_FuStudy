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
    

    

    [HttpPost("Like")]
        public async Task<IActionResult> Like([FromBody] QuestionRatingRequest questionRatingRequest)
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

        [HttpDelete("Unlike/{questionId}")]
        public async Task<IActionResult> Unlike(long questionId)
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
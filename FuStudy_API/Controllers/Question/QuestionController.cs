using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using CoreApiResponse;
using FuStudy_Service.Interface;
using FuStudy_Repository.Entity;
using FuStudy_Model.DTO.Response;
using FuStudy_Model.DTO.Request;
using Tools;


namespace FuStudy_API.Controllers.Question

{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : BaseController
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            this._questionService = questionService;
        }


        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return CustomResult("Data loaded!", questions);
        }


        [HttpGet("GetQuestionById/{id}")]
        public async Task<IActionResult> GetQuestionById(long id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return CustomResult("Question not found", HttpStatusCode.NotFound);
            }

            return CustomResult("Data loaded!", question);

        }

    [HttpPost("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionRequest questionRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return CustomResult(ModelState, HttpStatusCode.BadRequest);
                }


                var createdQuestion = await _questionService.CreateQuestionAsync(questionRequest);



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
                var updateQuestion = await _questionService.UpdateQuestionAsync(questionRequest, questionId);
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
                await _questionService.DeleteQuestionAsync(questionId);
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



}

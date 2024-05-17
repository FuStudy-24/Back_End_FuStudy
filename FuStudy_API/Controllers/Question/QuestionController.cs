using Microsoft.AspNetCore.Mvc;
using System;
using FuStudy_Service.Interface;
using FuStudy_Repository.Entity;
using FuStudy_Model.DTO.Respone;
using FuStudy_Model.DTO.Request;


namespace FuStudy_API.Controllers.Question

{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            this._questionService = questionService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionResponse>>> GetAllQuestions()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(questions);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionResponse>> GetQuestionById(long id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);

        }

        [HttpPost()]
        public async Task<ActionResult<QuestionResponse>> CreateQuestion([FromBody] QuestionRequest questionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createdQuestion = await _questionService.CreateQuestionAsync(questionRequest);
            
            
            
            return CreatedAtAction(nameof(GetQuestionById), new {id = createdQuestion}, createdQuestion);
        }

        [HttpPost("questionId")]
        public async Task<ActionResult<QuestionResponse>> UpdateQuestion(long questionId, [FromBody] QuestionRequest questionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                

            var updateQuestion = await _questionService.UpdateQuestionAsync(questionRequest, questionId);
            return CreatedAtAction(nameof(GetQuestionById), new { id = updateQuestion.Id }, updateQuestion);
        }

        [HttpDelete("questionId")]
        public async Task<ActionResult<QuestionResponse>> DeleteQuestion(long questionId)
        {
            await _questionService.DeleteQuestionAsync(questionId);

            return NoContent();
        }

        

    }



}

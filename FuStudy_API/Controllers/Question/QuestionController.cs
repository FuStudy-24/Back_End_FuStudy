using Microsoft.AspNetCore.Mvc;
using System;
using FuStudy_Service.Interface;
using FuStudy_Repository.Entity;


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

        [HttpGet("GetAllQuestion")]
        public async Task<IActionResult> GetAllQuestion() 
        {
            
                var questtions = await _questionService.GetAllQuestions();
                return Ok(questtions);
       
            
            
        }

 
        
    }
}
    


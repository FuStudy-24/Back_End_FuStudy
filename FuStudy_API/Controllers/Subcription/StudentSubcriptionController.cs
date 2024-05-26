using System;
using CoreApiResponse;
using FuStudy_Service.Interfaces;
using FuStudy_Service.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace FuStudy_API.Controllers.Subcription
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSubcriptionController : BaseController
    {
        private readonly IStudentSubcriptionService _studentSubcriptionService;

        public StudentSubcriptionController(IStudentSubcriptionService studentSubcriptionService)
        {
            _studentSubcriptionService = studentSubcriptionService;
        }

        [HttpGet("GetAllStudentSubcription")]
        public async Task<IActionResult> GetAllStudentSubcription()
        {
            try
            {
                var studentsubcrption = await _studentSubcriptionService.GetAllStudentSubcription();
                return CustomResult("Load Data Successful", studentsubcrption, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}

﻿using System;
using CoreApiResponse;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Service.Interfaces;
using FuStudy_Service.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Tools;

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
        public async Task<IActionResult> GetAllStudentSubcription([FromQuery] QueryObject queryObject)
        {
            try
            {
                var studentsubcrption = await _studentSubcriptionService.GetAllStudentSubcription(queryObject);
                return CustomResult("Load Data Successful", studentsubcrption, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("StudentSubcription/{id}")]
        public async Task<IActionResult> GetStudentSubcriptionByID(long id)
        {
            try
            {
                var studentsubcription = await _studentSubcriptionService.GetStudentSubcriptionByID(id);
                return CustomResult("ID has Found", studentsubcription, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("findStudentSubcription/{userId}")]
        public async Task<IActionResult> GetStudentSubcriptionByUserID(long userId)
        {
            try
            {
                var studentsubcription = await _studentSubcriptionService.GetStudentSubcriptionByUserID(userId);
                return CustomResult("ID has Found", studentsubcription, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }


        [HttpPost("CreateStudentSubcription")]
        public async Task<IActionResult> CreateStudentSubcription([FromBody] CreateStudentSubcriptionRequest studentSubcriptionRequest)
        {
            try
            {
                StudentSubcriptionResponse studentSubcriptionResonse = await _studentSubcriptionService.CreateStudentSubcription(studentSubcriptionRequest);
                return CustomResult("Created Successfully", studentSubcriptionResonse, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch("UpdateStudentSubcription/{id}")]
        public async Task<IActionResult> UpdateStudentSubcription(long id,[FromBody] UpdateStudentSubcriptionRequest updateStudentSubcriptionRequest)
        {
            try
            {
                StudentSubcriptionResponse studentSubcriptionResonse = await _studentSubcriptionService.UpdateStudentSubcription(id ,updateStudentSubcriptionRequest);
                return CustomResult("Updated Successfully", studentSubcriptionResonse, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch("DeleteStudentSubcription/{id}")]
        public async Task<IActionResult> DeleteStudentSubcription(long id, [FromBody] UpdateStudentSubcriptionRequest updateStudentSubcriptionRequest)
        {
            try
            {
                StudentSubcriptionResponse studentSubcriptionResonse = await _studentSubcriptionService.DeleteStudentSubcription(id, updateStudentSubcriptionRequest);
                return CustomResult("Deleted Successfully", studentSubcriptionResonse, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}

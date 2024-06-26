﻿using CoreApiResponse;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Service.Interface;
using FuStudy_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tools;

namespace FuStudy_API.Controllers.Major
{
    [Route("api/[controller]")]
    [ApiController]
    public class MentorMajorController : BaseController
    {
           private readonly IMentorMajorService _mentorMajorService;

        public MentorMajorController(IMentorMajorService mentorMajorService)
        {
            _mentorMajorService = mentorMajorService;
        }

        [HttpGet("GetAllMentorMajor")]
        public async Task<IActionResult> GetAllMentorMajor([FromQuery] QueryObject queryPbject)
        {
            try
            {
                var mms = await _mentorMajorService.GetAllMentorMajor(queryPbject);
                return CustomResult("Data Load Successfully", mms);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetAllMentorMajorByMentorId/{id}")]
        public async Task<IActionResult> GetAllMentorMajorByMentorId(long id)
        {
            try
            {
                var majors = await _mentorMajorService.GetAllMentorMajorByMentorId(id);

                return CustomResult("Data Load Successfully", majors);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetAllMentorMajorByMajorId/{id}")]
        public async Task<IActionResult> GetAllMentorMajorByMajorId(long id)
        {
            try
            {
                var mentors = await _mentorMajorService.GetAllMentorMajorByMajorId(id);

                return CustomResult("Data Load Successfully", mentors);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetMentorMajorById/{id}")]
        public async Task<IActionResult> GetMentorMajorById(long id)
        {
            try
            {
                var mm = await _mentorMajorService.GetMentorMajorById(id);

                return CustomResult("Data Load Successfully", mm);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("CreateMentorMajor")]
        public async Task<IActionResult> CreateMentorMajor(MentorMajorRequest mentorMajorRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return CustomResult(ModelState, HttpStatusCode.BadRequest);
                }

                MentorMajorResponse mm = await _mentorMajorService.CreateMentorMajor(mentorMajorRequest);
                return CustomResult("Create Successful", mm, HttpStatusCode.OK);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }

        }

        [HttpDelete("DeleteMentorMajor/{id}")]
        public async Task<IActionResult> DeleteMentorMajor(long id)
        {
            try
            {
                var mm = await _mentorMajorService.DeleteMentorMajor(id);
                return CustomResult("Delete Role Successfull (Status)", mm, HttpStatusCode.OK);
            }
            catch (CustomException.DataNotFoundException ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
            }

        }
    }
}

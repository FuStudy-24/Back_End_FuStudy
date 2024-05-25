using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CoreApiResponse;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FuStudy_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService authenticationService, IMapper _mapper)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateAccountDTORequest createAccountDTORequest)
        {
            try
            {
                CreateAccountDTOResponse user = await _authenticationService.Register(createAccountDTORequest);

                return CustomResult("Register Success",user, HttpStatusCode.OK);

            }
            catch (Exception e)
            {
                return CustomResult(e.Message, HttpStatusCode.InternalServerError);
            }
            
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTORequest loginDtoRequest)
        {
            try
            {
                (string, LoginDTOResponse) tuple = await _authenticationService.Login(loginDtoRequest);
                if (tuple.Item1 == null)
                {
                    return Unauthorized();
                }

                Dictionary<string, object> result = new()
                {
                    { "token", tuple.Item1 },
                    { "user", tuple.Item2 ?? null }
                };
                return CustomResult("Login Success",result, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message, HttpStatusCode.InternalServerError);
            }

            
        }
    }
}

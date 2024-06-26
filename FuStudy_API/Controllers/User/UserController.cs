using System.Net;
using CoreApiResponse;
using FuStudy_Model.DTO.Request;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tools;

namespace FUStudy_API.Controllers.User;

public class UserController : BaseController
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Authorize]
    [HttpGet("GetLoginUser")]
    public async Task<IActionResult> GetLoginUser()
    {
        try
        {
            var user = await _userService.GetLoginUser();
            return CustomResult("Get User Success", user, HttpStatusCode.OK);
        }
        catch (CustomException.InvalidDataException ex)
        {
            return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
        }
        catch (Exception exception)
        {
            return CustomResult(exception.Message, HttpStatusCode.InternalServerError);
        }
    }

    [Authorize]
    [HttpPatch("UpdateLoginUser")]
    public async Task<IActionResult> UpdateLoginUser([FromBody]UpdateAccountDTORequest updateAccountDtoRequest)
    {
        try
        {
            var user = await _userService.UpdateLoginUser(updateAccountDtoRequest);
            return CustomResult("Update Successful!", user);
        }
        catch (CustomException.InvalidDataException ex)
        {
            return CustomResult(ex.Message, HttpStatusCode.InternalServerError);
        }
        catch (Exception exception)
        {
            return CustomResult(exception.Message, HttpStatusCode.InternalServerError);
        }
    }

    
}
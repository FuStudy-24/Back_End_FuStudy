using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using Microsoft.Extensions.Configuration;
using Tools;

namespace FuStudy_Service.Service;

public class AuthenticationService: IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
        
    public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IUserService userService)
    {
        _unitOfWork = unitOfWork; 
        _mapper = mapper;
        _configuration = configuration;
        _userService = userService;
    }  
    public async Task<CreateAccountDTOResponse> Register(CreateAccountDTORequest createAccountDTORequest)
    {
        IEnumerable<User> checkEmail =
            await _unitOfWork.UserRepository.GetByFilterAsync(x => x.Email.Equals(createAccountDTORequest.Email));
        IEnumerable<User> checkUsername =
            await _unitOfWork.UserRepository.GetByFilterAsync(x => x.Username.Equals(createAccountDTORequest.Username));
        if (checkEmail.Any())
        {
            throw new CustomException.InvalidDataException("500","Email already exists.");
        }

        if (checkUsername.Any())
        {
            throw new InvalidDataException("Username already exists.");
        }
        var user = _mapper.Map<User>(createAccountDTORequest);
/*			user.permission_id = (await _userPermissionRepository.GetByFilterAsync(r => r.role.Equals("Customer"))).First().id;
*/
        
        user.Password = EncryptPassword.Encrypt(createAccountDTORequest.Password);
        user.Status = true;
        user.CreateDate = DateTime.Now.Date;
        user.RoleId = 3;
        
        
        
			
			
        await _unitOfWork.UserRepository.AddAsync(user);
        CreateAccountDTOResponse createAccountDTOResponse = _mapper.Map<CreateAccountDTOResponse>(user);
        return createAccountDTOResponse;
        
    }

    public async Task<(string, LoginDTOResponse)> Login(LoginDTORequest loginDtoRequest)
    {
        string hashedPass = EncryptPassword.Encrypt(loginDtoRequest.Password);
        IEnumerable<User> check = await _unitOfWork.UserRepository.GetByFilterAsync(x =>
            x.Username.Equals(loginDtoRequest.Username)
            && x.Password.Equals(hashedPass)
        );
        if (!check.Any())
        {
            throw new CustomException.InvalidDataException(HttpStatusCode.BadRequest.ToString(),$"Username or password error");
        }

        User user = check.First();
        if (user.Status == false)
        {
            throw new CustomException.InvalidDataException(HttpStatusCode.BadRequest.ToString(),$"User is not active");
        }

        LoginDTOResponse loginDtoResponse = _mapper.Map<LoginDTOResponse>(user);
        Authentication authentication = new(_configuration, _unitOfWork);
        string token = await authentication.GenerateJwtToken(user, 15);
        return (token, loginDtoResponse);
    }

    public async Task<RegisterTutorResponse> RegisterTutor(RegisterTutorRequest registerTutorRequest)
    {
        var mentor = _mapper.Map<Mentor>(registerTutorRequest);
        mentor.UserId = long.Parse(_userService.GetUserID());
        mentor.Status = "pending";
        await _unitOfWork.MentorRepository.AddAsync(mentor);
        RegisterTutorResponse registerTutorResponse = _mapper.Map<RegisterTutorResponse>(mentor);
        return registerTutorResponse;
    }

    public async Task<Token> SaveToken(Token token)
    {
        token.Time = DateTime.Now;
        token.Revoked = false;
        token.IsExpired = false;
        var tokenAdd = await _unitOfWork.TokenRepository.AddAsync(token);
        return tokenAdd;
    }

    public async Task<ResponseDTO> ResetPassAsync(UserResetPassDTO userReset)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(userReset.Email);
        if (user == null)
        {
            return new ResponseDTO()
            {
                Success = false,
                Message = "User not found"
            };
        }

        var userToken = await _unitOfWork.TokenRepository.GetUserToken(user.Id);
        if (userToken == null || userToken.IsExpired || userToken.Revoked)
        {
            return new ResponseDTO()
            {
                Success = false,
                Message = "Invalid or expired token"
            };
        }

        if (!userReset.Token.Equals(userToken.TokenValue))
        {
            return new ResponseDTO()
            {
                Success = false,
                Message = "Token not found"
            };
        }

        string encryptedPassword = EncryptPassword.Encrypt(userReset.NewPassword);
        user.Password = encryptedPassword;
        userToken.IsExpired = true;
        userToken.Revoked = true;
        

        
           await _unitOfWork.UserRepository.UpdateAsync(user);
           await _unitOfWork.TokenRepository.UpdateAsync(userToken);
            

            return new ResponseDTO()
            {
                Success = true,
                Message = "Password reset successfully"
            };
        
        
    }

}
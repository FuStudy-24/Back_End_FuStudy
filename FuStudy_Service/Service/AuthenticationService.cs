using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Model.Enum;
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
    public async Task<CreateAccountDTOResponse> Register(RegisterRequest registerRequest)
    {
        IEnumerable<User> checkEmail =
            await _unitOfWork.UserRepository.GetByFilterAsync(x => x.Email.Equals(registerRequest.Email));
        IEnumerable<User> checkUsername =
            await _unitOfWork.UserRepository.GetByFilterAsync(x => x.Username.Equals(registerRequest.Username));
        if (checkEmail.Count() != 0)
        {
            throw new InvalidDataException($"Email is exist");
        }

        if (checkUsername.Count() != 0)
        {
            throw new InvalidDataException($"Username is exist");
        }

        if (registerRequest.Password != registerRequest.ConfirmPassword)
        {
            throw new CustomException.InvalidDataException("Confirm Password not match!");
        }

        var userGender = "Order";
        switch (registerRequest.Gender.Trim().ToLower())
        {
            case "male": userGender = "Male"; break;
            case "female": userGender = "Female"; break;
        }
        
        var user = _mapper.Map<User>(registerRequest);
/*			user.permission_id = (await _userPermissionRepository.GetByFilterAsync(r => r.role.Equals("Customer"))).First().id;
*/
        user.Gender = userGender;
        //default avatar
        user.Avatar =
            "https://firebasestorage.googleapis.com/v0/b/artworks-sharing-platform.appspot.com/o/images%2F1.image.png?alt=media&token=3c77475f-90df-4f19-879e-c8024ae789b";
        user.Password = EncryptPassword.Encrypt(registerRequest.Password);
        user.Status = true;
        var role = _unitOfWork.RoleRepository.Get(role => role.RoleName.Trim().ToLower() == registerRequest.RoleName.Trim().ToLower())
            .FirstOrDefault();
        if (role == null)
        {
            throw new CustomException.InvalidDataException( "500", "This role name does not exist");
        }
        user.RoleId = role.Id;
        user.CreateDate = DateTime.Now.Date;





        
        await _unitOfWork.UserRepository.AddAsync(user);
        var roleName = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        var wallet = new Wallet
        {
            UserId = user.Id,
            Balance = 0,
            Status = true
        };
        await _unitOfWork.WalletRepository.AddAsync(wallet);
        if (roleName.RoleName == RoleName.Student.ToString())
        {
            var student = new Student();
            student.UserId = user.Id;
            await _unitOfWork.StudentRepository.AddAsync(student);
            
        }

        if (roleName.RoleName == RoleName.Mentor.ToString() )
        {
            var mentor = new Mentor
            {
                UserId = user.Id,
                AcademicLevel = "",
                WorkPlace = "",
                VerifyStatus = false,
                OnlineStatus = OnlineStatus.Invisible.ToString(),
                Skill = "",
                Video = ""
            };
            
            
            await _unitOfWork.MentorRepository.AddAsync(mentor);
            
        }
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
        loginDtoResponse.RoleName = _unitOfWork.RoleRepository.GetByID(user.RoleId).RoleName;
        Authentication authentication = new(_configuration, _unitOfWork);
        string token = await authentication.GenerateJwtToken(user, 15);
        return (token, loginDtoResponse);
    }

    public async Task<RegisterTutorResponse> RegisterTutor(RegisterTutorRequest registerTutorRequest)
    {
        var mentor = _mapper.Map<Mentor>(registerTutorRequest);
        mentor.UserId = long.Parse(_userService.GetUserID());
        mentor.VerifyStatus = true;
        await _unitOfWork.MentorRepository.AddAsync(mentor);
        RegisterTutorResponse registerTutorResponse = _mapper.Map<RegisterTutorResponse>(mentor);
        return registerTutorResponse;
    }

    public async Task<Token> SaveToken(Token token)
    {
        var existingToken = await _unitOfWork.TokenRepository.GetUserToken(token.UserId);

        if (existingToken != null)
        {
            // Mark existing token as expired
            existingToken.IsExpired = true;
            existingToken.Revoked = true;
            await _unitOfWork.TokenRepository.UpdateAsync(existingToken);
        }
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
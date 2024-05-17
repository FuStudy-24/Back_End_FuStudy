using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Repository;
using FuStudy_Repository.Entity;
using FuStudy_Service.Interface;
using Tools;

namespace FuStudy_Service.Service;

public class AuthenticationService: IAuthenticationService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public AuthenticationService(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<User> Register(CreateAccountDTORequest createAccountDTORequest)
    {
        IEnumerable<User> checkEmail =
            await _unitOfWork.UserRepository.GetByFilterAsync(x => x.Email.Equals(createAccountDTORequest.Email));
        IEnumerable<User> checkUsername =
            await _unitOfWork.UserRepository.GetByFilterAsync(x => x.Username.Equals(createAccountDTORequest.Username));
        if (checkEmail.Any())
        {
            // Kiểm tra permission_id
            foreach (var userCheck in checkEmail)
            {
                if (userCheck.Email == createAccountDTORequest.Email)
                {
                    throw new InvalidDataException($"Email is exist.");
                }
            }
        }
        if (checkUsername.Count() != 0)
        {
            throw new InvalidDataException($"Username is exist");
        }
        var user = _mapper.Map<User>(createAccountDTORequest);
/*			user.permission_id = (await _userPermissionRepository.GetByFilterAsync(r => r.role.Equals("Customer"))).First().id;
*/			
        user.Password = EncryptPassword.Encrypt(createAccountDTORequest.Password);
        user.Status = true;
        user.CreateDate = DateTime.Now.Date;
        
			
			
        await _unitOfWork.UserRepository.AddAsync(user);
        return user;
        
    }
}
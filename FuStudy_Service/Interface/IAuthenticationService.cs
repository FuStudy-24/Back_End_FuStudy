using FuStudy_Model.DTO.Request;
using FuStudy_Repository.Entity;

namespace FuStudy_Service.Interface;

public interface IAuthenticationService
{
    Task<User> Register(CreateAccountDTORequest createAccountDTORequest); 
    
}
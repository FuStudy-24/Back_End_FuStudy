using System.Threading.Tasks;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;

namespace FuStudy_Service.Interface;

public interface IAuthenticationService
{
    Task<CreateAccountDTOResponse> Register(CreateAccountDTORequest createAccountDTORequest); 
    Task<(string, LoginDTOResponse)> Login(LoginDTORequest loginDtoRequest);
    Task<RegisterTutorResponse> RegisterTutor(RegisterTutorRequest registerTutorRequest); 
    
}


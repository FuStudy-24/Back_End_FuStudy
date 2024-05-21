using FuStudy_Model.DTO.Request;
using FuStudy_Repository.Entity;
using FuStudy_Service.Interface;

namespace FuStudy_Service.Service;

public class UserService : IUserService
{
    public Task<User> CreateUser(CreateAccountDTORequest createAccountRequest)
    {
        throw new NotImplementedException();
    }
}
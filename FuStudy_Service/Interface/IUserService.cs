using System.Collections.Generic;
using System.Threading.Tasks;
using FuStudy_Model.DTO.Request;
using FuStudy_Repository.Entity;

namespace FuStudy_Service.Interface;

public interface IUserService
{
    Task<User> CreateUser(CreateAccountDTORequest createAccountRequest);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(long id);
    Task<User> UpdateUser(long id, UpdateAccountDTORequest updateAccountDTORequest);
    string GetUserID();
}
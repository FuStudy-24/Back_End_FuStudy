using System.Collections.Generic;
using System.Threading.Tasks;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using Tools;

namespace FuStudy_Service.Interface;

public interface IUserService
{
    Task<User> CreateUser(CreateAccountDTORequest createAccountRequest);
    Task<IEnumerable<UserDTOResponse>> GetAllUsers(QueryObject queryObject);
    Task<User> GetUserById(long id);
    Task<User> UpdateUser(long id, UpdateAccountDTORequest updateAccountDTORequest);
    string GetUserID();
    Task<UserDTOResponse> GetLoginUser();
    
    Task<UserDTOResponse> UpdateLoginUser(UpdateAccountDTORequest updateAccountDTORequest);
    
    Task<UserDTOResponse> UpdateLoginUserAvatar(ImageRequest imageRequest);


}
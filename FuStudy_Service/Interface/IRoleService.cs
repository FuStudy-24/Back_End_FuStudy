using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Interface
{
    public interface IRoleService
    {
        Task<List<RoleResponse>> GetAllRole(QueryObject queryObject);
        Task<RoleResponse> GetRoledById(long id);
        Task<RoleResponse> CreateRole(RoleRequest roleRequest);
        Task<RoleResponse> UpdateRole(long id, RoleRequest roleRequest);
        Task<bool> DeleteRole(long id);
    }
}

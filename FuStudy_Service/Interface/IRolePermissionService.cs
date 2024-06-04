using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace FuStudy_Service.Interface
{
    public interface IRolePermissionService
    {
        Task<List<RolePermissionResponse>> GetAllRolePermission(QueryObject queryObject);
        Task<List<RolePermissionResponse>> GetAllPermissionByRoleId(long id);
        Task<RolePermissionResponse> GetRolePermissionById(long id);
        Task<RolePermissionResponse> CreateRolePermission(RolePermissionRequest rolePermissionRequest);
        Task<bool> DeleteRolePermission(long id);
    }
}

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
    public interface IPermissionService
    {
        Task<List<PermissionResponse>> GetAllPermission(QueryObject queryObject);
        Task<PermissionResponse> GetPermissionById(long id);
        Task<PermissionResponse> CreatePermission(PermissionRequest permissionRequest);
        Task<PermissionResponse> UpdatePermission(long id, PermissionRequest permissionRequest);
        Task<bool> DeletePermission(long id);
    }
}

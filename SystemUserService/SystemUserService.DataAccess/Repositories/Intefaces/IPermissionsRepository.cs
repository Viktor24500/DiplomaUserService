using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Permissions;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
    public interface IPermissionsRepository
    {
        Task<Result<List<PermissionDTO>>> GetAllPermissions();
        Task<Result<PermissionDTO>> GetPermission(int id);
        Task<Result<PermissionDTO>> UpdatePermission(int id, string name);
        Task<Result<PermissionDTO>> CreatePermission(string name);

        Task<Result<PermissionDTO>> GetPermissionByName(string name);
    }
}

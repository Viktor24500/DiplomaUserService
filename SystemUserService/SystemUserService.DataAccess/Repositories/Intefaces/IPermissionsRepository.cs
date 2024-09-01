using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Permissions;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
    public interface IPermissionsRepository
    {
        Task<Result<List<PermissionDTO>>> GetAllPermissions();
        Task<Result<PermissionDTO>> GetPermission(int id);
        Task<Result> UpdatePermission(int id, string name, string? description);
        Task<ResultValueType<int>> CreatePermission(string name, string? description);

        Task<Result<PermissionDTO>> GetPermissionByName(string name);
    }
}

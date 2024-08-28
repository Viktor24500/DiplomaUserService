using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<Result<List<Permission>>> GetAllPermissions();

        Task<Result<Permission>> GetPermission(int id);
        Task<Result<Permission>> UpdatePermission(int id, string name);

        Task<Result<Permission>> CreatePermission(string name);
    }
}

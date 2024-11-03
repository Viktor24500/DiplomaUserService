using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Parametrs.Permissions;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<Result<List<Permission>>> GetAllPermissions();

        Task<Result<Permission>> GetPermission(int id);
        Task<Result<Permission>> UpdatePermission(PermissionUpdateParametrs updateParam);

        Task<Result<Permission>> CreatePermission(PermissionCreateParametrs createParam);
    }
}

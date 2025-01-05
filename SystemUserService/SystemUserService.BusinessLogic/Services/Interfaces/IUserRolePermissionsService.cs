using SystemUserService.BusinessLogic.Entities.UsersRolesPermissions;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IUserRolePermissionsService
    {
        Task<Result<List<UserRolePermissions>>> GetAllUserRolePermissions();

        Task<Result<List<UserRolePermissions>>> GetUserRolePermissionsByRoleId(int id);
        Task<Result<List<UserRolePermissions>>> GetUserRolePermissionsByUserId(int id);
        Task<Result<List<UserRolePermissions>>> GetUserRolePermissionsByPermissionId(int id);

    }
}

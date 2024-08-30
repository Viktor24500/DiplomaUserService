using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.UsersRolesPermissions;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
    public interface IUserRolePermissionsRepository
    {
        Task<Result<List<UserRolePermissionDTO>>> GetAllUserRolePermissions();

        Task<Result<List<UserRolePermissionDTO>>> GetUserRolePermissionsByRoleId(int id);
        Task<Result<List<UserRolePermissionDTO>>> GetUserRolePermissionsByUserId(int id);
        Task<Result<List<UserRolePermissionDTO>>> GetUserRolePermissionsByPermissionId(int id);
    }
}

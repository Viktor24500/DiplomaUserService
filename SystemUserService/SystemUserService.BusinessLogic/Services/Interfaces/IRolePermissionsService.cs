using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IRolePermissionsService
    {
        public Task<Result<List<RolesPermission>>> GetAllRolePermissions();
        public Task<Result<List<RolesPermission>>> GetRolePermissionsByRoleId(int id);
        public Task<Result<List<RolesPermission>>> CreateRolePermissions(int roleId, List<int> permissionsId);
        public Task<Result<List<RolesPermission>>> UpdateRolePermissions(int rolePermissionId, int roleId, int permissionId);
    }
}

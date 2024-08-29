using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.RolesPermissions;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
    public interface IRolePermissionsRepository
    {
        public Task<Result<List<RolePermissionsDTO>>> GetAllRolePermissions();
        public Task<Result<List<RolePermissionsDTO>>> GetRolePermissionsByRoleId(int id);
        public Task<Result> CreateRolePermissions(int roleId, List<int> permissionsId);
        public Task<Result> UpdateRolePermissions(int rolePermissionId, int roleId, int permissionsId);
    }
}

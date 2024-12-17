using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.BusinessLogic.Parametrs.RolePermission;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
	public interface IRolePermissionsService
	{
		public Task<Result<List<RolesPermission>>> GetAllRolePermissions();
		public Task<Result<RolesPermission>> GetRolePermissionsByRoleId(int id);
		public Task<Result<RolesPermission>> CreateRolePermissions(RolePermissionCreateParameters rolePermissionCreateParameters);
		public Task<Result<RolesPermission>> UpdateRolePermissions(RolePermissionUpdateParameters rolePermissionUpdateParameters);
	}
}

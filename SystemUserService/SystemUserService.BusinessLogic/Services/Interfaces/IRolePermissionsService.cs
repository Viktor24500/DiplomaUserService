using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.BusinessLogic.Parametrs.RolePermission;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
	public interface IRolePermissionsService
	{
		public Task<Result<List<RolePermission>>> GetAllRolePermissions();
		public Task<Result<RolePermission>> GetRolePermissionsByRoleId(int id);
		public Task<Result<RolePermission>> CreateRolePermissions(RolePermissionCreateParameters rolePermissionCreateParameters);
		public Task<Result<RolePermission>> UpdateRolePermissions(RolePermissionUpdateParameters rolePermissionUpdateParameters);
	}
}

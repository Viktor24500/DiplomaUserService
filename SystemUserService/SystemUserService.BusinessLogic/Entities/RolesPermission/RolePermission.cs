using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Entities.Roles;

namespace SystemUserService.BusinessLogic.Entities.RolesPermission
{
	public class RolePermission
	{
		public RolePermission(int rolePermissionId, int rolePermissionroleId, int rolePermissionPermissionId, int roleId,
			string roleName, string? roleDescription, List<Permission> permissions)
		{
			RolePermissionId = rolePermissionId;
			RoleId = rolePermissionroleId;
			PermissionId = rolePermissionPermissionId;
			Role = new Role(roleId, roleName, roleDescription);
			Permissions = permissions;
		}
		public int RolePermissionId { get; set; }
		public int RoleId { get; set; }
		public Role Role { get; set; }
		public int PermissionId { get; set; }
		public List<Permission> Permissions { get; set; }
	}
}

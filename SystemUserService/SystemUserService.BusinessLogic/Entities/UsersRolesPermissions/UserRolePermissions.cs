using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Entities.Users;

namespace SystemUserService.BusinessLogic.Entities.UsersRolesPermissions
{
	public class UserRolePermissions
	{
		public UserRolePermissions(int userRoleId, int userRolesUserId, int userRolesRoleId, Role role,
			User user, List<Permission> permission)
		{
			UserRoleId = userRoleId;
			UserRolesUserId = userRolesUserId;
			UserRolesRoleId = userRolesRoleId;
			User = user;
			Role = role;
			Permission = permission;
		}

		public int UserRoleId { get; set; }
		public int UserRolesUserId { get; set; }
		public int UserRolesRoleId { get; set; }
		public User User { get; set; }

		public Role Role { get; set; }
		public List<Permission> Permission { get; set; }
	}
}

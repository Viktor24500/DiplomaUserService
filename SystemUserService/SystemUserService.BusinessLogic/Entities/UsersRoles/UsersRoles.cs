using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Entities.Users;

namespace SystemUserService.BusinessLogic.Entities.UsersRoles
{
	public class UserRole
	{
		public UserRole(int userRoleId, int userRolesUserId, int userRolesRoleId, Role role, User user)
		{
			UserRoleId = userRoleId;
			UserRolesUserId = userRolesUserId;
			UserRolesRoleId = userRolesRoleId;
			User = user;
			Role = role;
		}
		public int UserRoleId { get; set; }
		public int UserRolesUserId { get; set; }
		public int UserRolesRoleId { get; set; }
		public User User { get; set; }

		public Role Role { get; set; }
	}
}

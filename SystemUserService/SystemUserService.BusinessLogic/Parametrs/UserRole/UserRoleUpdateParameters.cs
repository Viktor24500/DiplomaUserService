namespace SystemUserService.BusinessLogic.Parametrs.UserRole
{
	public class UserRoleUpdateParameters
	{
		public UserRoleUpdateParameters(int userRoleId, int userId, int roleId)
		{
			UserRoleId = userRoleId;
			UserId = userId;
			RoleId = roleId;
		}
		public int UserRoleId { get; set; }
		public int UserId { get; set; }
		public int RoleId { get; set; }
	}
}

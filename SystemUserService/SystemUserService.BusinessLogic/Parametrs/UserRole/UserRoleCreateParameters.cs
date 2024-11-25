namespace SystemUserService.BusinessLogic.Parametrs.UserRole
{
	public class UserRoleCreateParameters
	{
		public UserRoleCreateParameters(int userId, int roleId)
		{
			UserId = userId;
			RoleId = roleId;
		}
		public int UserId { get; set; }
		public int RoleId { get; set; }
	}
}

namespace SystemUserService.Request.UserRole
{
	public class UserRoleCreateRequest
	{
		public UserRoleCreateRequest(int userId, int roleId)
		{
			UserId = userId;
			RoleId = roleId;
		}
		public int UserId { get; set; }
		public int RoleId { get; set; }
	}
}

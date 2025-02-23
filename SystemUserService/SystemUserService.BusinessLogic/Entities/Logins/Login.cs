namespace SystemUserService.BusinessLogic.Entities.Logins
{
	public class Login
	{
		public Login(int id, int roleId, DateTime tokenExpiration, string token)
		{
			Id = id;
			TokenExpiration = tokenExpiration;
			Token = token;
			RoleId = roleId;
		}
		public int Id { get; set; }

		public int RoleId { get; set; }
		public string Token { get; set; }
		public DateTime TokenExpiration { get; set; }
	}
}

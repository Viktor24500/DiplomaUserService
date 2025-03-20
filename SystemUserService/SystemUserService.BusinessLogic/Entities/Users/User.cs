namespace SystemUserService.BusinessLogic.Entities.Users
{
	public class User
	{
		public User(int userId, string username, string userPassword, string email, string firstName, string lastName,
			string? comment, DateTime dateRegistered, DateTime? lastLogin, DateTime? tokenExpiration, bool isActive, string? lastToken, string phoneNumber)
		{
			UserId = userId;
			Username = username;
			UserPassword = userPassword;
			Email = email;
			FirstName = firstName;
			LastName = lastName;
			Comment = comment;
			DateRegistered = dateRegistered;
			LastLogin = lastLogin;
			TokenExpiration = tokenExpiration;
			IsActive = isActive;
			LastToken = lastToken;
			PhoneNumber = phoneNumber;
		}

		public int UserId { get; set; }
		public string Username { get; set; }
		public string UserPassword { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? Comment { get; set; }
		public DateTime DateRegistered { get; set; }
		public DateTime? LastLogin { get; set; }
		public DateTime? TokenExpiration { get; set; }

		public string? LastToken { get; set; }
		public bool IsActive { get; set; }
		public string PhoneNumber { get; set; }
	}
}

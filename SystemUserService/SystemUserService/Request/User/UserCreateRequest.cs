namespace SystemUserService.Request.User
{
	public class UserCreateRequest
	{
		public UserCreateRequest(string username, string userPassword, string email, string firstName, string lastName, string? fatherName, bool isActive)
		{
			Username = username;
			UserPassword = userPassword;
			Email = email;
			FirstName = firstName;
			LastName = lastName;
			FatherName = fatherName;
			IsActive = isActive;
		}

		public string Username { get; set; }
		public string UserPassword { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? FatherName { get; set; }
		public bool IsActive { get; set; }
	}
}

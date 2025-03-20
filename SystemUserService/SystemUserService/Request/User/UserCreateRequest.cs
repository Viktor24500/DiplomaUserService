namespace SystemUserService.Request.User
{
	public class UserCreateRequest
	{
		public UserCreateRequest(string username, string userPassword, string email, string firstName, string lastName, string?
					comment, bool isActive, int roleId, string phoneNumber)
		{
			Username = username;
			FirstName = firstName;
			LastName = lastName;
			Comment = comment;
			IsActive = isActive;
			RoleId = roleId;
			UserPassword = userPassword;
			Email = email;
			PhoneNumber = phoneNumber;
		}
		public string Username { get; set; }
		public string UserPassword { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? Comment { get; set; }

		public int RoleId { get; set; }
		public bool IsActive { get; set; }

		public string PhoneNumber { get; set; }
	}
}

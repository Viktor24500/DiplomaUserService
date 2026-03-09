
namespace SystemUserService.Request.User
{
	public class UserCreateParameters
	{
		public UserCreateParameters(string username, string userPassword, string email, string firstName, string lastName, string?
			comment, bool isActive, DateTime dateRegistered, DateTime? lastLogin, string phoneNumber)
		{
			Username = username;
			FirstName = firstName;
			LastName = lastName;
			Comment = comment;
			IsActive = isActive;
			UserPassword = userPassword;
			Email = email;
			PhoneNumber = phoneNumber;
			DateRegistered = dateRegistered;
			LastLogin = lastLogin;
		}
		public string Username { get; set; }
		public string UserPassword { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? Comment { get; set; }

		public DateTime DateRegistered { get; set; }
		public DateTime? LastLogin { get; set; }
		public bool IsActive { get; set; }

		public string PhoneNumber { get; set; }
	}
}

namespace SystemUserService.Request.User
{
	public class UserUpdateParameters
	{
		public UserUpdateParameters(int id, string email, string firstName, string lastName, string? fatherName, bool isActive)
		{
			Id = id;
			Email = email;
			FirstName = firstName;
			LastName = lastName;
			FatherName = fatherName;
			IsActive = isActive;
		}

		public int Id { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? FatherName { get; set; }
		public bool IsActive { get; set; }
	}
}

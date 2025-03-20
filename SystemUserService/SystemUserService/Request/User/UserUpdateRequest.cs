namespace SystemUserService.Request.User
{
	public class UserUpdateRequest
	{
		public UserUpdateRequest(int id, string email, string firstName, string lastName, string? comment, bool isActive, string phoneNumber)
		{
			Id = id;
			Email = email;
			FirstName = firstName;
			LastName = lastName;
			Comment = comment;
			IsActive = isActive;
			PhoneNumber = phoneNumber;
		}

		public int Id { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? Comment { get; set; }
		public bool IsActive { get; set; }

		public string PhoneNumber { get; set; }
	}
}

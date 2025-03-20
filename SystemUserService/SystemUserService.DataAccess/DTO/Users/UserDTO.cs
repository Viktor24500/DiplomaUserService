namespace SystemUserService.DataAccess.DTO.Users
{
	public record UserDTO(int UserId, string Username, string UserPassword, string Email,
		string FirstName, string LastName, string? Comment, DateTime DateRegistered,
		DateTime? LastLogin, string? LastToken, DateTime? TokenExpiration, bool IsActive, string PhoneNumber);

}

using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.DataAccess.DTO.Users;

namespace SystemUserService.BusinessLogic.Extensions
{
	public static class UsersExtension
	{
		public static User MapToUser(this UserDTO userDTO)
		{
			return new User(userDTO.UserId, userDTO.Username, userDTO.UserPassword, userDTO.Email,
				userDTO.FirstName, userDTO.LastName, userDTO.Comment, userDTO.DateRegistered,
				userDTO.LastLogin, userDTO.TokenExpiration, userDTO.IsActive, userDTO.LastToken, userDTO.PhoneNumber);
		}
		public static List<User> MapToUsersCollection(this List<UserDTO> userDTOList)
		{
			IEnumerable<User> users = from userDTO in userDTOList
									  select new User(userDTO.UserId, userDTO.Username, userDTO.UserPassword, userDTO.Email,
				userDTO.FirstName, userDTO.LastName, userDTO.Comment, userDTO.DateRegistered,
				userDTO.LastLogin, userDTO.TokenExpiration, userDTO.IsActive, userDTO.LastToken, userDTO.PhoneNumber);
			return users.ToList();
		}
	}
}

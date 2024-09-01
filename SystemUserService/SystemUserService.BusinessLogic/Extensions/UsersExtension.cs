using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.DataAccess.DTO.Users;

namespace SystemUserService.BusinessLogic.Extensions
{
    public static class UsersExtension
    {
        public static User MapToUser(this UserDTO userDTO)
        {
            return new User(userDTO.UserId, userDTO.Username, userDTO.UserPassword, userDTO.Email,
                userDTO.FirstName, userDTO.LastName, userDTO.FatherName, userDTO.DateRegistered,
                userDTO.LastLogin, userDTO.IsActive, userDTO.LastToken);
        }
        public static List<User> MapToUsersCollection(this List<UserDTO> userDTOList)
        {
            IEnumerable<User> users = from userDTO in userDTOList
                                      select new User(userDTO.UserId, userDTO.Username, userDTO.UserPassword, userDTO.Email,
                userDTO.FirstName, userDTO.LastName, userDTO.FatherName, userDTO.DateRegistered,
                userDTO.LastLogin, userDTO.IsActive, userDTO.LastToken);
            return users.ToList();
        }
    }
}

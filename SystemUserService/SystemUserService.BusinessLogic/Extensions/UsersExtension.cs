using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.DataAccess.DTO.Users;

namespace SystemUserService.BusinessLogic.Extensions
{
    public static class UsersExtensions
    {
        public static User MapToUser(this UserDTO userDTO)
        {
            return new User(userDTO.UserId, userDTO.UserName, userDTO.UserPassword, userDTO.IsActive, userDTO.RoleId, userDTO.RoleName);
        }
        public static List<User> MapToUsersCollection(this List<UserDTO> usersDTOList)
        {
            IEnumerable<User> users = from userDTO in usersDTOList
                                      select new User(userDTO.UserId, userDTO.UserName,
                userDTO.UserPassword, userDTO.IsActive, userDTO.RoleId, userDTO.RoleName);
            return users.ToList();
        }
    }
}

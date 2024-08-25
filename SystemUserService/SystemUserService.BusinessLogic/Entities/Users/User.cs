using SystemUserService.BusinessLogic.Entities.Permissions;

namespace SystemUserService.BusinessLogic.Entities.Users
{
    public class User
    {
        public User(int userId, string userName, string userPassword, bool isActive, int roleId, string roleName)
        {
            UserID = userId;
            Username = userName;
            UserPassword = userPassword;
            IsActive = isActive;
            Role = new Permission(roleId, roleName);
        }
        public int UserID { get; set; }

        public string Username { get; set; }
        public string UserPassword { get; set; }

        public bool IsActive { get; set; }

        public Permission Role { get; set; }
    }
}

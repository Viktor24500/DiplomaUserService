using SystemUserService.BusinessLogic.Entities.Roles;

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
            Role = new Role(roleId, roleName);
        }
        public int UserID { get; set; }

        public string Username { get; set; }
        public string UserPassword { get; set; }

        public bool IsActive { get; set; }

        public Role Role { get; set; }
    }
}

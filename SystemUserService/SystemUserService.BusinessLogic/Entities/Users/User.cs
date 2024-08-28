namespace SystemUserService.BusinessLogic.Entities.Users
{
    public class User
    {
        public User(int userId, string username, string userPassword, string realName,
            string realSurname, string? realFatherName, DateTime registrationDate, bool isActive)
        {
            UserId = userId;
            Username = username;
            UserPassword = userPassword;
            RealName = realName;
            RealSurname = realSurname;
            RealFatherName = realFatherName;
            RegistrationDate = registrationDate;
            IsActive = isActive;
        }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string UserPassword { get; set; }

        public string RealName { get; set; }

        public string RealSurname { get; set; }

        public string? RealFatherName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsActive { get; set; }
    }

}

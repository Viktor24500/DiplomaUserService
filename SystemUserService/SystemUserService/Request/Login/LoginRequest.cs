namespace SystemUserService.Request.Login
{
    public class LoginRequest
    {
        public LoginRequest(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public string Name { get; set; }
        public string Password { get; set; }
    }
}

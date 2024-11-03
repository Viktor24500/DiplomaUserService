namespace SystemUserService.BusinessLogic.Parametrs.Login
{
    public class LoginParametrs
    {
        public LoginParametrs(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public string Name { get; set; }
        public string Password { get; set; }
    }
}

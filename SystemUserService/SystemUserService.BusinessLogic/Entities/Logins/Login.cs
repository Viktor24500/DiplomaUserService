namespace SystemUserService.BusinessLogic.Entities.Logins
{
    public class Login
    {
        public Login(int id, DateTime tokenExpiration, string token)
        {
            Id = id;
            TokenExpiration = tokenExpiration;
            Token = token;
        }
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}

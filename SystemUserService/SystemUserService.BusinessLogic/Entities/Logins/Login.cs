namespace SystemUserService.BusinessLogic.Entities.Logins
{
    public class Login
    {
        public Login(int id, DateTime tokenExpiration)
        {
            Id = id;
            TokenExpiration = tokenExpiration;
        }
        public int Id { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}

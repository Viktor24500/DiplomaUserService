using SystemUserService.BusinessLogic.Entities.Logins;
using SystemUserService.DataAccess.DTO.Login;

namespace SystemUserService.BusinessLogic.Extensions
{
    public static class LoginExtension
    {
        public static Login MapToLogin(this LoginDTO loginDTO)
        {
            return new Login(loginDTO.Id, loginDTO.TokenExpiration, loginDTO.Token);
        }
    }
}

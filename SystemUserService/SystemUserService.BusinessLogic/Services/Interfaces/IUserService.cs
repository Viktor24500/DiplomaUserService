using SystemUserService.BusinessLogic.Entities.Logins;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Parametrs.Login;
using SystemUserService.Common.Results;
using SystemUserService.Request.User;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
	public interface IUserService
	{
		Task<Result<List<User>>> GetAllUsers();
		Task<Result<User>> GetUser(int id);
		Task<Result<List<User>>> GetUserByActiveStatus(bool isActive);
		Task<Result<User>> UpdateUser(UserUpdateParameters userUpdateParam);
		Task<Result<User>> CreateUser(UserCreateParameters userCreateParam);

		Task<Result<User>> GetUserByName(string name);

		Task<Result<string>> LoginUser(LoginParametrs loginParam);
        Task<Result<Login>> GetUserByToken(string token);
    }
}

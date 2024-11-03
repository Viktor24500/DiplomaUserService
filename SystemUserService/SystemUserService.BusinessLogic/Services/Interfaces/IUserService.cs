using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Parametrs.Login;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<List<User>>> GetAllUsers();
        Task<Result<User>> GetUser(int id);
        Task<Result<List<User>>> GetUserByActiveStatus(bool isActive);
        Task<Result<User>> UpdateUser(int id, string email, string firstName, string lastName, string? fatherName, bool isActive);
        Task<Result<User>> CreateUser(string username, string userPassword, string email,
                       string firstName, string lastName, string? fatherName,
                       DateTime dateRegistered, DateTime? lastLogin, bool isActive);

        Task<Result<User>> GetUserByName(string name);

        Task<Result<string>> LoginUser(LoginParametrs loginParam);
    }
}

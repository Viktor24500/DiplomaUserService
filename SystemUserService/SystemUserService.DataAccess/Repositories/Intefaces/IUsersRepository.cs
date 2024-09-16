using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Users;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
    public interface IUsersRepository
    {
        Task<Result<List<UserDTO>>> GetAllUsers();
        Task<Result<UserDTO>> GetUser(int id);
        Task<Result<List<UserDTO>>> GetUserByActiveStatus(bool isActive);
        Task<Result<UserDTO>> GetUserByEmail(string email);
        Task<Result> UpdateUser(int id, string email, string firstName,
            string lastName, string? fatherName, bool isActive);
        Task<Result<int>> CreateUser(string username, string userPassword,
            string email, string firstName, string lastName, string? fatherName,
            DateTime dateRegistered, DateTime? lastLogin, string? lastToken, DateTime? tokenExpiration,
            bool isActive);

        Task<Result<UserDTO>> GetUserByName(string name);

        Task<Result> UpdateLoginUser(int id, DateTime? lastLogin, string? lastToken, DateTime? tokenExpiration);
    }
}

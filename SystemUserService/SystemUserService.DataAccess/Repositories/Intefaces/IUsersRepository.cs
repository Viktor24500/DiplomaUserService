using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Users;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
    public interface IUsersRepository
    {
        Task<Result<List<UserDTO>>> GetAllUsers();
        Task<Result<UserDTO>> GetUserByUserId(int id);
        Task<Result<List<UserDTO>>> GetUserByRoleId(int id);
        Task<Result<UserDTO>> UpdateUser(int userId, string userName, string userPassword, bool isActive, int roleId);
        Task<Result<UserDTO>> CreateUser(string userName, string userPassword, bool isActive, int roleId);

        Task<Result<UserDTO>> GetUserByUserName(string name);

        Task<Result<List<UserDTO>>> GetUserByRoleName(string name);
    }
}

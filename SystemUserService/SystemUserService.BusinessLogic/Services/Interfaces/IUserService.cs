using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Result<List<User>>> GetAllUsers();

        public Task<Result<User>> GetUserByUserId(int id);

        public Task<Result<User>> GetUserByUserName(string name);

        public Task<Result<User>> UpdateUser(int userId, string userName, string userPassword, bool isActive, int roleId);

        public Task<Result<User>> CreateUser(string userName, string userPassword, bool isActive, int roleId);

        public Task<Result<List<User>>> GetUserByRoleId(int id);

        public Task<Result<List<User>>> GetUserByRoleName(string name);
    }
}

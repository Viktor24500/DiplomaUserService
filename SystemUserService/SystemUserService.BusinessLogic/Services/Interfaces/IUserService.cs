using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Result<List<User>>> GetAllUsers();

        public Task<Result<User>> GetUserById(int id);

        public Task<Result<User>> GetUserByName(string name);

        public Task<Result<User>> UpdateUser(int userId, string userName, string userPassword, bool isActive, int roleId);

        public Task<Result<User>> CreateUser(string userName, string userPassword, bool isActive, int roleId);
    }
}

using Microsoft.Extensions.Configuration;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string? _connectionString;
        public UsersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.MainConnectionString);
        }

        public Task<Result<UserDTO>> CreateUser(string userName, string userPassword, bool isActive, int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<UserDTO>>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<UserDTO>>> GetUserByRoleId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<UserDTO>>> GetUserByRoleName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserDTO>> GetUserByUserId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserDTO>> GetUserByUserName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserDTO>> UpdateUser(int userId, string userName, string userPassword, bool isActive, int roleId)
        {
            throw new NotImplementedException();
        }
    }
}

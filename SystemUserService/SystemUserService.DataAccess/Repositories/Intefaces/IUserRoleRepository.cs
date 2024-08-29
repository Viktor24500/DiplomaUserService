using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.UsersRoles;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
    public interface IUserRoleRepository
    {
        public Task<Result<List<UserRoleDTO>>> GetAllUsersRoles();

        public Task<Result<List<UserRoleDTO>>> GetUserRoleByUserId(int id);
        public Task<Result<List<UserRoleDTO>>> GetUserRoleByRoleId(int id);
        public Task<Result> CreateUserRoles(int userId, List<int> roleId);
        public Task<Result> UpdateUserRole(int userRoleId, int userId, int roleId);
    }
}

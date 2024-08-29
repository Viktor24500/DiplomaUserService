using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IUserRoleService
    {
        public Task<Result<List<UserRole>>> GetAllUsersRoles();

        public Task<Result<List<UserRole>>> GetUserRoleByUserId(int id);
        public Task<Result<List<UserRole>>> GetUserRoleByRoleId(int id);
        public Task<Result<List<UserRole>>> CreateUserRoles(int userId, List<int> roleId);
        public Task<Result<List<UserRole>>> UpdateUserRole(int userRoleId, int userId, int roleId);
    }
}

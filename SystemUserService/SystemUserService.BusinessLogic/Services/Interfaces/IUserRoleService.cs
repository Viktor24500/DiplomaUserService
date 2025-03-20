using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.BusinessLogic.Parametrs.UserRole;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IUserRoleService
    {
        public Task<Result<List<UserRole>>> GetAllUsersRoles();

        public Task<Result<UserRole>> GetUserRoleByUserId(int id);
        public Task<Result<List<UserRole>>> GetUserRoleByRoleId(int id);
        public Task<Result<UserRole>> CreateUserRole(UserRoleCreateParameters userRoleCreateParam);
        public Task<Result<UserRole>> UpdateUserRole(UserRoleUpdateParameters userRoleUpdateParam);
		Task<Result<List<UserRole>>> SearchUserRoles(string name);
	}
}

using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.BusinessLogic.Parametrs.UserRole;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
	public interface IUserRoleService
	{
		public Task<Result<List<UserRole>>> GetAllUsersRoles();

		public Task<Result<List<UserRole>>> GetUserRoleByUserId(int id);
		public Task<Result<List<UserRole>>> GetUserRoleByRoleId(int id);
		public Task<Result<List<UserRole>>> CreateUserRole(UserRoleCreateParameters userRoleCreateParam);
		public Task<Result<List<UserRole>>> UpdateUserRole(UserRoleUpdateParameters userRoleUpdateParam);
	}
}

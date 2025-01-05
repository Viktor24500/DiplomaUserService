using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.UsersRoles;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
	public interface IUserRoleRepository
	{
		public Task<Result<List<UserRoleDTO>>> GetAllUsersRoles();

		public Task<Result<UserRoleDTO>> GetUserRoleByUserId(int id);
		public Task<Result<List<UserRoleDTO>>> GetUserRoleByRoleId(int id);
		public Task<Result> CreateUserRole(int userId, int roleId);
		public Task<Result> UpdateUserRole(int userRoleId, int userId, int roleId);

		public Task<Result<UserRoleDTO>> GetUserRoleByUserRoleId(int userRoleId);
	}
}

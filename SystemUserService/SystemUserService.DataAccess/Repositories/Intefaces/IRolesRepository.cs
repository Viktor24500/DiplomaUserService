using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Roles;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
    public interface IRolesRepository
    {
        Task<Result<List<RoleDTO>>> GetAllRoles();
        Task<Result<RoleDTO>> GetRole(int id);
        Task<Result> UpdateRole(int id, string name, string? description);
        Task<Result<int>> CreateRole(string name, string? description);

        Task<Result<RoleDTO>> GetRoleByName(string name);
    }
}

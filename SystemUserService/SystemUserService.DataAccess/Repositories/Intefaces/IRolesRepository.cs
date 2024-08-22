using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Roles;

namespace SystemUserService.DataAccess.Repositories.Intefaces
{
    public interface IRolesRepository
    {
        Task<Result<List<RoleDTO>>> GetAllRoles();
        Task<Result<RoleDTO>> GetRole(int id);
        Task<Result<RoleDTO>> UpdateRole(int ID, string name);
        Task<Result<RoleDTO>> CreateRole(string name);

        Task<Result<RoleDTO>> GetRoleByName(string name);
    }
}

using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Result<List<Role>>> GetAllRoles();

        Task<Result<Role>> GetRole(int id);
        Task<Result<Role>> UpdateRole(int id, string name);

        Task<Result<Role>> CreateRole(string name);
    }
}

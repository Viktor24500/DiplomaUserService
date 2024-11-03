using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Parametrs.Roles;
using SystemUserService.Common.Results;

namespace SystemUserService.BusinessLogic.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Result<List<Role>>> GetAllRoles();

        Task<Result<Role>> GetRole(int id);
        Task<Result<Role>> UpdateRole(RoleUpdateParametrs updateParam);

        Task<Result<Role>> CreateRole(RoleCreateParametrs createParam);
    }
}

using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.DataAccess.DTO.Roles;

namespace SystemUserService.BusinessLogic.Extensions
{
    public static class RolesExtension
    {
        public static Role MapToRole(this RoleDTO roleDTO)
        {
            return new Role(roleDTO.Id, roleDTO.Name);
        }
        public static List<Role> MapToRolesCollection(this List<RoleDTO> roleDTOList)
        {
            IEnumerable<Role> roles = from roleDTO in roleDTOList select new Role(roleDTO.Id, roleDTO.Name);
            return roles.ToList();
        }
    }
}

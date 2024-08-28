using System.Data;
using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.DataAccess.DTO.Permissions;

namespace SystemUserService.BusinessLogic.Extensions
{
    public static class PermissionsExtensions
    {
        public static Permission MapToPermission(this PermissionDTO permissionDTO)
        {
            return new Permission(permissionDTO.Id, permissionDTO.Name, permissionDTO.Description);
        }
        public static List<Permission> MapToPermissionsCollection(this List<PermissionDTO> permissionDTOList)
        {
            IEnumerable<Permission> permissions = from permissionDTO in permissionDTOList
                                                  select new Permission(permissionDTO.Id,
                permissionDTO.Name, permissionDTO.Description);
            return permissions.ToList();
        }
    }
}

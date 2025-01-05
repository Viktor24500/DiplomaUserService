using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.DataAccess.DTO.RolesPermissions;

namespace SystemUserService.BusinessLogic.Extensions
{
    public static class RolesPermissionsExtension
    {
        public static RolePermission MapToRolesPermissions(this RolePermissionsDTO rolePermissionsDTO)
        {
            List<Permission> permissions = new List<Permission>
            {
                new Permission(
                    rolePermissionsDTO.PermissionId,
                    rolePermissionsDTO.PermissionName,
                    rolePermissionsDTO.PermissionDescription
                )
            };
            return new RolePermission(
                rolePermissionsDTO.RolePermissionId,
                rolePermissionsDTO.RolePermissionRoleId,
                rolePermissionsDTO.RolePermissionPermissionId,
                rolePermissionsDTO.RoleId,
                rolePermissionsDTO.RoleName,
                rolePermissionsDTO.RoleDescription,
                permissions
            );
        }
        public static List<RolePermission> MapToRolesPermissionsCollection(this List<RolePermissionsDTO> rolePermissionsDTOList)
        {
            List<RolePermission> rolePermissionList = new List<RolePermission>();
            Dictionary<int, RolePermission> rolesPermissionMap = new Dictionary<int, RolePermission>();

            foreach (RolePermissionsDTO dto in rolePermissionsDTOList)
            {
                Role role = new Role(
                    dto.RoleId, dto.RoleName, dto.RoleDescription
                );

                Permission permission = new Permission(
                    dto.PermissionId, dto.PermissionName, dto.PermissionDescription
                );

                if (rolesPermissionMap.ContainsKey(dto.RolePermissionRoleId))
                {
                    rolesPermissionMap[dto.RolePermissionRoleId].Role = role;
                    rolesPermissionMap[dto.RolePermissionRoleId].Permissions.Add(permission);
                }
                else
                {
                    List<Permission> permissions = new List<Permission> { permission };

                    RolePermission rolesPermission = new RolePermission(
                        dto.RolePermissionId,
                        dto.RolePermissionRoleId,
                        dto.RolePermissionPermissionId,
                        dto.RoleId,
                        dto.RoleName,
                        dto.RoleDescription,
                        permissions
                    );

                    rolesPermissionMap[dto.RolePermissionId] = rolesPermission;
                }
            }

            rolePermissionList = rolesPermissionMap.Values.ToList();

            return rolePermissionList;

        }
    }
}

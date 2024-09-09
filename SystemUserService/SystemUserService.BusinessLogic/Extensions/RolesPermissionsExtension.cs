using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.DataAccess.DTO.RolesPermissions;

namespace SystemUserService.BusinessLogic.Extensions
{
    public static class RolesPermissionsExtension
    {
        public static RolesPermission MapToRolesPermissions(this RolePermissionsDTO rolePermissionsDTO)
        {
            List<Permission> permissions = new List<Permission>
            {
                new Permission(
                    rolePermissionsDTO.PermissionId,
                    rolePermissionsDTO.PermissionName,
                    rolePermissionsDTO.PermissionDescription
                )
            };
            return new RolesPermission(
                rolePermissionsDTO.RolePermissionId,
                rolePermissionsDTO.RolePermissionRoleId,
                rolePermissionsDTO.RolePermissionPermissionId,
                rolePermissionsDTO.RoleId,
                rolePermissionsDTO.RoleName,
                rolePermissionsDTO.RoleDescription,
                permissions
            );
        }
        public static List<RolesPermission> MapToRolesPermissionsCollection(this List<RolePermissionsDTO> rolePermissionsDTOList)
        {
            List<RolesPermission> rolePermissionList = new List<RolesPermission>();
            Dictionary<int, RolesPermission> rolesPermissionMap = new Dictionary<int, RolesPermission>();

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

                    RolesPermission rolesPermission = new RolesPermission(
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

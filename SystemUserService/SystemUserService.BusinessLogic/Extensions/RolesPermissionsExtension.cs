using SystemUserService.BusinessLogic.Entities.Permissions;
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
            // Create a new list to hold the mapped RolesPermission objects
            var rolesPermissionsList = new List<RolesPermission>();

            // Iterate over each RolePermissionsDTO in the input list
            foreach (var dto in rolePermissionsDTOList)
            {
                // Create a list of Permission from the DTO's permission information
                var permissions = new List<Permission>
                {
                    new Permission(dto.PermissionId, dto.PermissionName, dto.PermissionDescription)
                    // Add more permissions if necessary
                };

                // Map the DTO to a RolesPermission object
                var rolesPermission = new RolesPermission(
                    dto.RolePermissionId,
                    dto.RolePermissionRoleId,
                    dto.RolePermissionPermissionId,
                    dto.RoleId,
                    dto.RoleName,
                    dto.RoleDescription,
                    permissions
                );

                // Add the mapped RolesPermission object to the list
                rolesPermissionsList.Add(rolesPermission);
            }
            return rolesPermissionsList;
        }
    }
}

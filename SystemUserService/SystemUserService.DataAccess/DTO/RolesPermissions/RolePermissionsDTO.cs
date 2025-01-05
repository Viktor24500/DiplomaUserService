namespace SystemUserService.DataAccess.DTO.RolesPermissions
{
    public record RolePermissionsDTO(int RolePermissionId, int RolePermissionRoleId,
        int RolePermissionPermissionId, int RoleId, string RoleName, string? RoleDescription,
        int PermissionId, string PermissionName, string? PermissionDescription
    );

}

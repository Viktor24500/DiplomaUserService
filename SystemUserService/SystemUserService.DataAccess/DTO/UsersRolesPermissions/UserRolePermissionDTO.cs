namespace SystemUserService.DataAccess.DTO.UsersRolesPermissions
{
    public record UserRolePermissionDTO(int UserRoleId, int UserRolesUserId, int UserRolesRoleId, int RoleId,
    string RoleName, string? RoleDescription, int UserId, string Username, string UserPassword, string Email,
    string FirstName, string LastName, string? FatherName, DateTime DateRegistered, DateTime? LastLogin,
    string? LastToken, bool IsActive, int PermissionId, string PermissionName, string? PermissionDescription);
}

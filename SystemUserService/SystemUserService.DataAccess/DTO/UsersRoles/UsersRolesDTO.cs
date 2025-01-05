namespace SystemUserService.DataAccess.DTO.UsersRoles
{
    public record UserRoleDTO(
        int UserRoleId, int UserRolesUserId, int UserRolesRoleId, int RoleId,
        string RoleName, string? RoleDescription, int UserId, string Username,
        string UserPassword, string Email, string FirstName, string LastName,
        string? FatherName, DateTime DateRegistered, DateTime? LastLogin, DateTime? TokenExpiration, string? LastToken, bool IsActive);
}

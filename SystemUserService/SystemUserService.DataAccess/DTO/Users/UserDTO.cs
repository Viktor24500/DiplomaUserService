namespace SystemUserService.DataAccess.DTO.Users
{
    public record UserDTO(int UserId, string UserName, string UserPassword, bool IsActive, int RoleId, string RoleName);
}

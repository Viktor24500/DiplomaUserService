namespace SystemUserService.DataAccess.DTO.Users
{
    public record UsersDTO(int UserId, string UserName, string UserPassword, bool IsActive, int RoleId, string RoleName);
}

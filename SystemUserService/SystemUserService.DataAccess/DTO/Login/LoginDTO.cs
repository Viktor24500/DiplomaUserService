namespace SystemUserService.DataAccess.DTO.Login
{
	public record LoginDTO(int Id, int RoleId, string Token, DateTime TokenExpiration);
}

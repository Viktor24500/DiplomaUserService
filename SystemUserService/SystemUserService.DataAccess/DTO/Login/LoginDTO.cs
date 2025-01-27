namespace SystemUserService.DataAccess.DTO.Login
{
    public record LoginDTO(int Id, string Token, DateTime TokenExpiration);
}

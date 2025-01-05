namespace SystemUserService.Common.Enums
{
    public enum ErrorCodes : ushort
    {
        Success = 0,
        Created = 1,
        BadRequest = 2,
        Unauthorized = 3,
        Forbidden = 4,
        NotFound = 5,
        InternalServerError = 6,
        Conflict = 7,
    }
}

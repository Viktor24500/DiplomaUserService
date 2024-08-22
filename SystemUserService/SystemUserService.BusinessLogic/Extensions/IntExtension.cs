namespace SystemUserService.BusinessLogic.Extensions
{
    public static class IntExtension
    {
        public static bool IsNegative(this int value)
        {
            return value < 0;
        }
    }
}

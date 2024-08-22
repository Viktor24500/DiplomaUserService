namespace SystemUserService.BusinessLogic.Extensions
{
    public static class DecimalExtension
    {
        public static bool IsNegative(this decimal value)
        {
            return value < 0;
        }
    }
}

using SystemUserService.Common.Results;

namespace SystemUserService.Utility
{
    public static class Utilities
    {
        public static void HandleUnexpectedErrorCode(Result result)
        {
            throw new Exception($"Unexpected error code {result.ErrorCode}");
        }

        public static void HandleUnexpectedErrorCode<T>(Result<T> result)
        {
            throw new Exception($"Unexpected error code {result.ErrorCode}");
        }
    }
}

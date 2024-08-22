namespace SystemUserService.Common.Results
{
    public class Result
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Result<T> where T : class
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public T Data { get; set; }
    }
}

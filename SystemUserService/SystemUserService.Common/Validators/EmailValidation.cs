using Microsoft.Extensions.Logging;
using SystemUserService.Common.Results;

namespace SystemUserService.Common.Validators
{
    public class EmailValidation
    {
        private ILogger<EmailValidation> _logger;
        EmailValidation(ILogger<EmailValidation> logger)
        {
            _logger = logger;
        }
        public Result isEmailValid(string email)
        {
            Result result = new Result();
            return result;
        }
    }
}

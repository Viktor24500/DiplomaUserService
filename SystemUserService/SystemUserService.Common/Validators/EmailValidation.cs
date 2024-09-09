using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using SystemUserService.Common.Enums;
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
            Regex emailPattern = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailPattern.IsMatch(email))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "email not match regex";
                return result;
            }
            return result;
        }
    }
}

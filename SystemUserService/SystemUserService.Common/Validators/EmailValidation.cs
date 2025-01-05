using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;

namespace SystemUserService.Common.Validators
{
    public class EmailValidation
    {
        private ILogger<EmailValidation> _logger;
        public EmailValidation(ILogger<EmailValidation> logger)
        {
            _logger = logger;
        }
        public Result isEmailPatternValid(string email)
        {
            Result result = new Result();
            Regex emailPattern = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailPattern.IsMatch(email))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "email not match regex";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            return result;
        }
    }
}

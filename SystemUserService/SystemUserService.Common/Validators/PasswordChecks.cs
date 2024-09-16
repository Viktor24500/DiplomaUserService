using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;


namespace SystemUserService.Common.Validators
{
    public class PasswordChecks
    {
        private ILogger<PasswordChecks> _logger;
        public PasswordChecks(ILogger<PasswordChecks> logger)
        {
            _logger = logger;
        }
        public Result isPasswordPatternValid(string password)
        {
            //Check password pattern
            //Regex hasNumberAndLetter = new Regex(@"\w+"); //has more than 1 number and letter


            Result result = new Result();
            if (!hasNumber(password))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "password must contains at least 1 number";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (!hasLetter(password))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "password must contains at least 1 small and capital letter";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (!hasMinimum8Chars(password))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "password must contains at least 8 chars";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            return result;
        }
        private bool hasNumber(string password)
        {
            Regex hasNumber = new Regex(@"[0-9]+"); //has more than 1 number
            if (!hasNumber.IsMatch(password))
            {
                return false;
            }
            return true;

        }

        private bool hasLetter(string password)
        {
            Regex hasLetter = new Regex(@"[A-Z]+[a-z]+"); // has more than 1 capital and small letter
            if (!hasLetter.IsMatch(password))
            {
                return false;
            }
            return true;
        }

        private bool hasMinimum8Chars(string password)
        {
            Regex hasMinimum8Chars = new Regex(@".{8,}"); //has minimum 8 char
            if (!hasMinimum8Chars.IsMatch(password))
            {
                return false;
            }
            return true;
        }
    }
}

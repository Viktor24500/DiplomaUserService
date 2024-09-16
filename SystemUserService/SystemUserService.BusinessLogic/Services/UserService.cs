using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Extensions;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Common.Validators;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private IUsersRepository _usersRepository;
        private PasswordChecks _passwordChecks;
        private EmailValidation _emailValidation;
        private ILogger<UserService> _logger;
        public UserService(IUsersRepository usersRepository, ILogger<UserService> logger, PasswordChecks passwordChecks, EmailValidation emailValidation)
        {
            _usersRepository = usersRepository;
            _logger = logger;
            _passwordChecks = passwordChecks;
            _emailValidation = emailValidation;
        }
        public async Task<Result<User>> CreateUser(string username, string userPassword, string email, string firstName, string lastName, string? fatherName, DateTime dateRegistered, DateTime? lastLogin, bool isActive)
        {
            Result<User> result = new Result<User>();
            //TODO YP: всі ці перевірки треба винести в приватні методи з відповідними назвами 
            //для того щоб це все краще читалось
            if (string.IsNullOrWhiteSpace(username))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (string.IsNullOrWhiteSpace(userPassword))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "password can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //check password pattern
            //TODO YP: назва методу не відповідає тому що він робить. З назви здається що він валідує пасворд а насправді він тільки валідує патерн
            if (_passwordChecks.isPasswordValid(userPassword).ErrorCode == (int)ErrorCodes.BadRequest)
            {
                result.ErrorCode = _passwordChecks.isPasswordValid(userPassword).ErrorCode;
                result.ErrorMessage = _passwordChecks.isPasswordValid(userPassword).ErrorMessage;
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "email can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "firstName or lastName can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //check email pattern
            if (_emailValidation.isEmailValid(email).ErrorCode == (int)ErrorCodes.BadRequest)
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "email not match with pattern";
            }
            Result<UserDTO> repResult = await _usersRepository.GetUserByName(username);
            if (repResult.ErrorCode == (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"User with name {username} exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            repResult = await _usersRepository.GetUserByEmail(email);
            if (repResult.ErrorCode == (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"User with email {email} exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            string hashedPassword;
            //Hash password 
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the password to a byte array
                byte[] bytes = Encoding.UTF8.GetBytes(userPassword);

                // Compute the hash
                byte[] hashBytes = sha256Hash.ComputeHash(bytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Converts byte to hexadecimal string
                }
                hashedPassword = sb.ToString();
            }
            //TODO YP: токен і його екпірейшен не являються частиною профайла юзера, вони являються частино
            //логін сесії і оброблятися повинні окремо
            string? lastToken = repResult.Data.LastToken;
            DateTime? tokenExpiration = repResult.Data.TokenExpiration;
            Result<int> repCreateResult = await _usersRepository.CreateUser(username, hashedPassword, email, firstName, lastName, fatherName, dateRegistered,
            lastLogin, lastToken, tokenExpiration, isActive);
            if (repCreateResult.ErrorCode == (int)ErrorCodes.Success)
            {
                repResult = await _usersRepository.GetUser(repCreateResult.Data);
                result.Data = repResult.Data.MapToUser();
            }
            return result;
        }

        public async Task<Result<List<User>>> GetAllUsers()
        {
            Result<List<UserDTO>> repResult = await _usersRepository.GetAllUsers();
            Result<List<User>> result = new Result<List<User>>();
            result.Data = repResult.Data.MapToUsersCollection();
            return result;
        }

        public async Task<Result<User>> GetUser(int id)
        {
            Result<User> result = new Result<User>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<UserDTO> repResult = await _usersRepository.GetUser(id);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"User with {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToUser();
            return result;
        }

        public async Task<Result<List<User>>> GetUserByIsActive(bool isActive)
        {
            Result<List<User>> result = new Result<List<User>>();
            Result<List<UserDTO>> repResult = await _usersRepository.GetUserByIsActive(isActive);
            result.Data = repResult.Data.MapToUsersCollection();
            return result;
        }

        public async Task<Result<User>> GetUserByName(string name)
        {
            Result<User> result = new Result<User>();
            if (string.IsNullOrWhiteSpace(name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<UserDTO> repResult = await _usersRepository.GetUserByName(name);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"User with {name} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToUser();
            return result;
        }

        public async Task<Result<string>> LoginUser(string name, string password)
        {
            //TODO YP: тут краще винести все в приватні методи з навами щоб було читабельне флоу
            Result<string> result = new Result<string>();
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "username and password can't be empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<UserDTO> repResult = await _usersRepository.GetUserByName(name);
            if (repResult.ErrorCode != (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "invalid username or password";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //check password pattern
            if (_passwordChecks.isPasswordValid(password).ErrorCode == (int)ErrorCodes.BadRequest)
            {
                result.ErrorCode = _passwordChecks.isPasswordValid(password).ErrorCode;
                result.ErrorMessage = _passwordChecks.isPasswordValid(password).ErrorMessage;
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            string hashedPassword;
            //Hash password 
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the password to a byte array
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash
                byte[] hashBytes = sha256Hash.ComputeHash(bytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Converts byte to hexadecimal string
                }
                hashedPassword = sb.ToString();
            }

            if (repResult.Data.UserPassword != hashedPassword)
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "invalid username or password";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            //TODO YP: я вже писав що це не повинно бути частиною юзера це повинні бути сесії юзера і зберігатись в окремій таблиці
            string token = Guid.NewGuid().ToString();
            DateTime lastLogin = DateTime.Now;
            //TODO YP: це повинно бути конфігурабельно
            DateTime tokenExpiration = lastLogin.AddMinutes(30);

            int userId = repResult.Data.UserId;
            Result repUpdateLoginResult = await _usersRepository.UpdateLoginUser(userId, lastLogin, token, tokenExpiration);

            if (repUpdateLoginResult.ErrorCode == (int)ErrorCodes.Success)
            {
                repResult = await _usersRepository.GetUser(userId);
                if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
                {
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    result.ErrorMessage = $"User with item {userId} not exist";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
                result.Data = repResult.Data.LastToken;
            }
            return result;
        }

        public async Task<Result<User>> UpdateUser(int id, string email, string firstName, string lastName, string? fatherName, bool isActive)
        {
            Result<User> result = new Result<User>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "email can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "firstName or lastName can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //check email pattern
            if (_emailValidation.isEmailValid(email).ErrorCode == (int)ErrorCodes.BadRequest)
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "email not match with pattern";
            }

            Result<UserDTO> repResult = await _usersRepository.GetUserByEmail(email);
            if (repResult.ErrorCode != (int)ErrorCodes.NotFound)
            {
                if (repResult.Data.UserId != id)
                {
                    result.ErrorCode = (int)ErrorCodes.Conflict;
                    result.ErrorMessage = $"User with email {email} exist";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
            }


            Result repUpdateResult = await _usersRepository.UpdateUser(id, email, firstName, lastName, fatherName, isActive);
            if (repUpdateResult.ErrorCode == (int)ErrorCodes.Success)
            {
                repResult = await _usersRepository.GetUser(id);
                if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
                {
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    result.ErrorMessage = $"User with item {id} not exist";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
                result.Data = repResult.Data.MapToUser();
                return result;
            }
            return result;
        }
    }
}

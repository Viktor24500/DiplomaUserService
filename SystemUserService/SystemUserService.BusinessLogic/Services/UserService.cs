using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using SystemUserService.BusinessLogic.Entities.Logins;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Extensions;
using SystemUserService.BusinessLogic.Parametrs.Login;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Common.Validators;
using SystemUserService.DataAccess.DTO.Login;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.Repositories.Intefaces;
using SystemUserService.Request.User;

namespace SystemUserService.BusinessLogic.Services
{
	public class UserService : IUserService
	{
		private IUsersRepository _usersRepository;
		private PasswordChecks _passwordChecks;
		private EmailValidation _emailValidation;
		private ILogger<UserService> _logger;
		private IConfiguration _configuration;
		public UserService(IUsersRepository usersRepository, ILogger<UserService> logger, PasswordChecks passwordChecks, EmailValidation emailValidation,
			IConfiguration configuration)
		{
			_usersRepository = usersRepository;
			_logger = logger;
			_passwordChecks = passwordChecks;
			_emailValidation = emailValidation;
			_configuration = configuration;
		}
		public async Task<Result<User>> CreateUser(UserCreateParameters userCreateParam)
		{
			Result<User> result = new Result<User>();
			//TODO YP: всі ці перевірки треба винести в приватні методи з відповідними назвами 
			//для того щоб це все краще читалось
			if (string.IsNullOrWhiteSpace(userCreateParam.Username))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "name can't be null or empty";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			if (string.IsNullOrWhiteSpace(userCreateParam.UserPassword))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "password can't be null or empty";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			//check password pattern
			if (_passwordChecks.isPasswordPatternValid(userCreateParam.UserPassword).ErrorCode == (int)ErrorCodes.BadRequest)
			{
				result.ErrorCode = _passwordChecks.isPasswordPatternValid(userCreateParam.UserPassword).ErrorCode;
				result.ErrorMessage = _passwordChecks.isPasswordPatternValid(userCreateParam.UserPassword).ErrorMessage;
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			if (string.IsNullOrWhiteSpace(userCreateParam.Email))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "email can't be null or empty";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			if (string.IsNullOrWhiteSpace(userCreateParam.FirstName) || string.IsNullOrWhiteSpace(userCreateParam.LastName))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "firstName or lastName can't be null or empty";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			//check email pattern
			if (_emailValidation.isEmailPatternValid(userCreateParam.Email).ErrorCode == (int)ErrorCodes.BadRequest)
			{

				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "email not match with pattern";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<UserDTO> repResult = await _usersRepository.GetUserByName(userCreateParam.Username);
			if (repResult.ErrorCode == (int)ErrorCodes.Success)
			{
				_logger.LogError(repResult.ErrorMessage);
				result.ErrorCode = (int)ErrorCodes.Conflict;
				result.ErrorMessage = $"User with name {userCreateParam.Username} exist";
				return result;
			}
			repResult = await _usersRepository.GetUserByEmail(userCreateParam.Email);
			if (repResult.ErrorCode == (int)ErrorCodes.Success)
			{
				result.ErrorCode = (int)ErrorCodes.Conflict;
				result.ErrorMessage = $"User with email {userCreateParam.Email} exist";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			//Hash password 
			string hashedPassword = HashPassword(userCreateParam.UserPassword);

			//TODO YP: токен і його екпірейшен не являються частиною профайла юзера, вони являються частино
			//логін сесії і оброблятися повинні окремо
			string? lastToken;
			DateTime? tokenExpiration;
			if (repResult.Data != null)
			{
				lastToken = repResult.Data.LastToken;
				tokenExpiration = repResult.Data.TokenExpiration;
			}
			else
			{
				lastToken = null;
				tokenExpiration = null;
			}
			Result<int> repCreateResult = await _usersRepository.CreateUser(userCreateParam.Username, hashedPassword,
				userCreateParam.Email, userCreateParam.FirstName, userCreateParam.LastName, userCreateParam.FatherName, userCreateParam.DateRegistered,
			userCreateParam.LastLogin, lastToken, tokenExpiration, userCreateParam.IsActive);
			if (repCreateResult.ErrorCode == (int)ErrorCodes.Success)
			{
				repResult = await _usersRepository.GetUser(repCreateResult.Data);
				if (repCreateResult.ErrorCode == (int)ErrorCodes.NotFound)
				{
					result.ErrorCode = (int)ErrorCodes.NotFound;
					result.ErrorMessage = $"User with item {repCreateResult.Data} not exist";
					_logger.LogError(result.ErrorMessage);
					return result;
				}
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

		public async Task<Result<List<User>>> GetUserByActiveStatus(bool isActive)
		{
			Result<List<User>> result = new Result<List<User>>();
			Result<List<UserDTO>> repResult = await _usersRepository.GetUserByActiveStatus(isActive);
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

		public async Task<Result<Login>> LoginUser(LoginParametrs loginParam)
		{
			//TODO YP: тут краще винести все в приватні методи з навами щоб було читабельне флоу
			Result<Login> result = new Result<Login>();
			if (string.IsNullOrWhiteSpace(loginParam.Name) || string.IsNullOrWhiteSpace(loginParam.Password))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "username and password can't be empty";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			Result<UserDTO> repResult = await _usersRepository.GetUserByName(loginParam.Name);
			if (repResult.ErrorCode != (int)ErrorCodes.Success)
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = $"User with {loginParam.Name} not exist";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			//check password pattern
			if (_passwordChecks.isPasswordPatternValid(loginParam.Password).ErrorCode == (int)ErrorCodes.BadRequest)
			{
				result.ErrorCode = _passwordChecks.isPasswordPatternValid(loginParam.Password).ErrorCode;
				result.ErrorMessage = _passwordChecks.isPasswordPatternValid(loginParam.Password).ErrorMessage;
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			string hashedPassword = HashPassword(loginParam.Password);

			if (repResult.Data.UserPassword != hashedPassword)
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "invalid password";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			//TODO YP: я вже писав що це не повинно бути частиною юзера це повинні бути сесії юзера і зберігатись в окремій таблиці
			string token = Guid.NewGuid().ToString();
			DateTime lastLogin = DateTime.Now;

			DateTime tokenExpiration = lastLogin.AddMinutes(double.Parse(_configuration["TokenExpirationTime"]));

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
				result.Data = new Login(userId, tokenExpiration, repResult.Data.LastToken);
			}
			return result;
		}

		public async Task<Result<User>> UpdateUser(UserUpdateParameters userUpdateParam)
		{
			Result<User> result = new Result<User>();
			if (IntExtension.IsNegative(userUpdateParam.Id))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			if (string.IsNullOrWhiteSpace(userUpdateParam.Email))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "email can't be null or empty";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			if (string.IsNullOrWhiteSpace(userUpdateParam.FirstName) || string.IsNullOrWhiteSpace(userUpdateParam.LastName))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "firstName or lastName can't be null or empty";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			//check email pattern
			if (_emailValidation.isEmailPatternValid(userUpdateParam.Email).ErrorCode == (int)ErrorCodes.BadRequest)
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "email not match with pattern";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			Result<UserDTO> repResult = await _usersRepository.GetUserByEmail(userUpdateParam.Email);
			if (repResult.ErrorCode != (int)ErrorCodes.NotFound)
			{
				if (repResult.Data.UserId != userUpdateParam.Id)
				{
					result.ErrorCode = (int)ErrorCodes.Conflict;
					result.ErrorMessage = $"User with email {userUpdateParam.Email} exist";
					_logger.LogError(result.ErrorMessage);
					return result;
				}
			}


			Result repUpdateResult = await _usersRepository.UpdateUser(userUpdateParam.Id, userUpdateParam.Email,
				userUpdateParam.FirstName, userUpdateParam.LastName, userUpdateParam.FatherName, userUpdateParam.IsActive);
			if (repUpdateResult.ErrorCode == (int)ErrorCodes.Success)
			{
				repResult = await _usersRepository.GetUser(userUpdateParam.Id);
				if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
				{
					result.ErrorCode = (int)ErrorCodes.NotFound;
					result.ErrorMessage = $"User with item {userUpdateParam.Id} not exist";
					_logger.LogError(result.ErrorMessage);
					return result;
				}
				result.Data = repResult.Data.MapToUser();
			}
			return result;
		}

		public async Task<Result<Login>> GetUserByToken(string token)
		{
			Result<Login> result = new Result<Login>();
			if (string.IsNullOrWhiteSpace(token))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "token can't be null or empty";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<LoginDTO> repResult = await _usersRepository.GetUserByToken(token);
			if (repResult.ErrorCode == (int)ErrorCodes.Success)
			{
				result.Data = repResult.Data.MapToLogin();
			}
			return result;
		}

		private string HashPassword(string password)
		{
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
			return hashedPassword;
		}
	}
}

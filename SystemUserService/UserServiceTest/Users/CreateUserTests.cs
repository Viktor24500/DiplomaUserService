using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Services;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Common.Validators;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.Repositories.Intefaces;
using SystemUserService.Request.User;

namespace UserServiceTest.Users
{
	[TestClass]
	public class CreateUserTests
	{
		private Mock<IUsersRepository> _mockUsersRepository;
		private Mock<ILogger<UserService>> _mockLogger;
		private UserService _userService;
		private PasswordChecks _passwordChecks;
		private Mock<ILogger<PasswordChecks>> _mockPasswordLogger;
		private EmailValidation _emailValidation;
		private Mock<ILogger<EmailValidation>> _mockEmailValidationLogger;

		[TestInitialize]
		public void Setup()
		{
			_mockUsersRepository = new Mock<IUsersRepository>();
			_mockLogger = new Mock<ILogger<UserService>>();
			_mockPasswordLogger = new Mock<ILogger<PasswordChecks>>();
			_mockEmailValidationLogger = new Mock<ILogger<EmailValidation>>();
			_passwordChecks = new PasswordChecks(_mockPasswordLogger.Object);
			_emailValidation = new EmailValidation(_mockEmailValidationLogger.Object);

			var mockConfiguration = new Mock<IConfiguration>();

			_userService = new UserService(
				_mockUsersRepository.Object,
				_mockLogger.Object,
				_passwordChecks,
				_emailValidation,
				mockConfiguration.Object
				);
		}

		[TestMethod]
		public async Task CreateUser_Fail_EmptyUsername()
		{
			// Arrange
			UserCreateParameters userParams = new UserCreateParameters("", "ValidPass1", "test@example.com", "John", "Doe", null, true, DateTime.UtcNow, null, "0966345678");

			// Act
			Result<User> result = await _userService.CreateUser(userParams);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("name can't be null or empty", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUser_Fail_EmptyPassword()
		{
			// Arrange
			UserCreateParameters userParams = new UserCreateParameters("testuser", "", "test@example.com", "John", "Doe", null, true, DateTime.UtcNow, null, "0966345678");

			// Act
			Result<User> result = await _userService.CreateUser(userParams);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("password can't be null or empty", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUser_Fail_InvalidPasswordPattern()
		{
			// Arrange
			UserCreateParameters userParams = new UserCreateParameters("testuser", "short", "test@example.com", "John", "Doe", null, true, DateTime.UtcNow, null, "0966345678");

			// Act
			Result<User> result = await _userService.CreateUser(userParams);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual(_passwordChecks.isPasswordPatternValid(userParams.UserPassword).ErrorMessage, result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUser_Fail_EmptyEmail()
		{
			// Arrange
			UserCreateParameters userParams = new UserCreateParameters("testuser", "ValidPass1", "", "John", "Doe", null, true, DateTime.UtcNow, null, "0966345678");

			// Act
			Result<User> result = await _userService.CreateUser(userParams);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("email can't be null or empty", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUser_Fail_EmailPatternInvalid()
		{
			// Arrange
			UserCreateParameters userParams = new UserCreateParameters("testuser", "ValidPass1", "invalidemail", "John", "Doe", null, true, DateTime.UtcNow, null, "0966345678");

			// Act
			Result<User> result = await _userService.CreateUser(userParams);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("email not match with pattern", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUser_Fail_UsernameConflict()
		{
			// Arrange
			UserCreateParameters userParams = new UserCreateParameters("existingUser", "ValidPass1", "test@example.com", "John",
				"Doe", null, true, DateTime.UtcNow, null, "0966345678");
			Result<UserDTO> repoResult = new Result<UserDTO>();
			repoResult.ErrorCode = (int)ErrorCodes.Success;

			_mockUsersRepository.Setup(repo => repo.GetUserByName(userParams.Username))
				.ReturnsAsync(repoResult);

			// Act
			Result<User> result = await _userService.CreateUser(userParams);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Conflict, result.ErrorCode);
			Assert.AreEqual($"User with name {userParams.Username} exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUser_Fail_EmailConflict()
		{
			// Arrange
			var userParams = new UserCreateParameters("testuser", "ValidPass1", "existing@example.com", "John", "Doe", null, true, DateTime.UtcNow, null, "0966345678");
			Result<UserDTO> repoResult = new Result<UserDTO>();
			repoResult.ErrorCode = (int)ErrorCodes.Success;

			Result<UserDTO> getUserByUsernameResult = new Result<UserDTO>();
			getUserByUsernameResult.ErrorCode = (int)ErrorCodes.NotFound;

			_mockUsersRepository.Setup(repo => repo.GetUserByEmail(userParams.Email))
				.ReturnsAsync(repoResult);

			_mockUsersRepository.Setup(repo => repo.GetUserByName(userParams.Username))
				.ReturnsAsync(getUserByUsernameResult);

			// Act
			Result<User> result = await _userService.CreateUser(userParams);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Conflict, result.ErrorCode);
			Assert.AreEqual($"User with email {userParams.Email} exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUser_Success()
		{
			// Arrange
			var userParams = new UserCreateParameters("testuser", "ValidPass1", "test@example.com", "John", "Doe", null, true, DateTime.UtcNow, null, "0966345678");
			Result<UserDTO> getUserByUsernameResult = new Result<UserDTO>();
			getUserByUsernameResult.ErrorCode = (int)ErrorCodes.NotFound;

			Result<UserDTO> getUserByEmail = new Result<UserDTO>();
			getUserByEmail.ErrorCode = (int)ErrorCodes.NotFound;

			UserDTO resDTO = new UserDTO(1, "testuser", "", "test@example.com", "John", "Doe", null, DateTime.UtcNow, null, null, null, true, "0966345678");
			Result<UserDTO> expectedResult = new Result<UserDTO>();
			expectedResult.ErrorCode = (int)ErrorCodes.Success;
			expectedResult.Data = resDTO;

			Result<int> resCreateUser = new Result<int>();
			resCreateUser.ErrorCode = (int)ErrorCodes.Success;
			resCreateUser.Data = 1;

			_mockUsersRepository.Setup(repo => repo.GetUserByName(userParams.Username))
				.ReturnsAsync(getUserByUsernameResult);

			_mockUsersRepository.Setup(repo => repo.GetUserByEmail(userParams.Email))
				.ReturnsAsync(getUserByEmail);

			_mockUsersRepository.Setup(repo => repo.CreateUser(
				userParams.Username,
				"cbd9babc2b9ebfd53e4ff9b94dc5cc1fdb8f71ad428bbc7c2529bb71c522f69b", // Password will be hashed
				userParams.Email,
				userParams.FirstName,
				userParams.LastName,
				userParams.Comment,
				userParams.DateRegistered,
				userParams.LastLogin,
				null, // Token
				null, // Token expiration
				userParams.IsActive,
				"0966345678"))
				.ReturnsAsync(resCreateUser);

			_mockUsersRepository.Setup(repo => repo.GetUser(1))
				.ReturnsAsync(expectedResult);

			// Act
			Result<User> result = await _userService.CreateUser(userParams);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.AreEqual(userParams.Username, result.Data.Username);
			Assert.AreEqual(userParams.Email, result.Data.Email);
		}
	}

}

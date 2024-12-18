using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemUserService.BusinessLogic.Parametrs.Login;
using SystemUserService.BusinessLogic.Services;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Common.Validators;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace UserServiceTest.Users
{
	[TestClass]
	public class LoginTests
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

			// Dependencies that are not actively tested here can be mocked/null for simplicity
			Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
			Dictionary<string, string> inMemorySettings = new Dictionary<string, string>();
			inMemorySettings.Add("TokenExpirationTime", "60");// Token expires in 60 minutes

			_mockConfiguration.Setup(c => c["TokenExpirationTime"])
							  .Returns(inMemorySettings["TokenExpirationTime"]);

			_userService = new UserService(
				_mockUsersRepository.Object,
				_mockLogger.Object,
				_passwordChecks,
				_emailValidation,
				_mockConfiguration.Object);
		}

		[TestMethod]
		public async Task LoginUser_Fail_EmptyUsernameOrPassword()
		{
			// Arrange
			LoginParametrs loginParam = new LoginParametrs("", "");

			// Act
			Result<string> result = await _userService.LoginUser(loginParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("username and password can't be empty", result.ErrorMessage);
		}

		[TestMethod]
		public async Task LoginUser_Fail_UserNotFound()
		{
			// Arrange
			LoginParametrs loginParam = new LoginParametrs("unknown.user", "SomePassword");

			Result<UserDTO> repoResult = new Result<UserDTO>();
			repoResult.ErrorCode = (int)ErrorCodes.BadRequest;
			repoResult.ErrorMessage = "User with unknown.user not exist";

			_mockUsersRepository.Setup(repo => repo.GetUserByName(loginParam.Name))
								.ReturnsAsync(repoResult);

			// Act
			Result<string> result = await _userService.LoginUser(loginParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual($"User with {loginParam.Name} not exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task LoginUser_Fail_PasswordMustContainsAtLeastEightChars()
		{
			// Arrange
			LoginParametrs loginParam = new LoginParametrs("user1", "Some12");

			Result<UserDTO> repoResult = new Result<UserDTO>();
			UserDTO user = new UserDTO(
				1, "user1", "Some", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true
				);
			repoResult.Data = user;
			_mockUsersRepository.Setup(repo => repo.GetUserByName(loginParam.Name))
								.ReturnsAsync(repoResult);

			// Act
			Result<string> result = await _userService.LoginUser(loginParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual($"password must contains at least 8 chars", result.ErrorMessage);
		}

		[TestMethod]
		public async Task LoginUser_Fail_PasswordMustContainsAtLeastOneNumber()
		{
			// Arrange
			LoginParametrs loginParam = new LoginParametrs("user1", "Some");

			Result<UserDTO> repoResult = new Result<UserDTO>();
			UserDTO user = new UserDTO(
				1, "user1", "Some", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true
				);
			repoResult.Data = user;
			_mockUsersRepository.Setup(repo => repo.GetUserByName(loginParam.Name))
								.ReturnsAsync(repoResult);

			// Act
			Result<string> result = await _userService.LoginUser(loginParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("password must contains at least 1 number", result.ErrorMessage);
		}

		[TestMethod]
		public async Task LoginUser_Fail_PasswordMustContainsAtLeastOneSmallAndCapitalLetter()
		{
			// Arrange
			LoginParametrs loginParam = new LoginParametrs("user1", "12131");

			Result<UserDTO> repoResult = new Result<UserDTO>();
			UserDTO user = new UserDTO(
				1, "user1", "Some", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true
				);
			repoResult.Data = user;
			_mockUsersRepository.Setup(repo => repo.GetUserByName(loginParam.Name))
								.ReturnsAsync(repoResult);

			// Act
			Result<string> result = await _userService.LoginUser(loginParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("password must contains at least 1 small and capital letter", result.ErrorMessage);
		}

		[TestMethod]
		public async Task LoginUser_Success()
		{
			// Arrange
			LoginParametrs loginParam = new LoginParametrs("user", "Some2323Password");

			Result<UserDTO> repoResult = new Result<UserDTO>();
			UserDTO user = new UserDTO(
				1, "user", "b440d283f448be5df4322477fc648ed8dfd5c4c983f0703ab84bec787b61799b", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true
				);
			repoResult.Data = user;
			_mockUsersRepository.Setup(repo => repo.GetUserByName(loginParam.Name))
								.ReturnsAsync(repoResult);
			_mockUsersRepository.Setup(repo => repo.GetUser(1))
					.ReturnsAsync(repoResult);

			Result loginRequest = new Result();

			_mockUsersRepository.Setup(repo => repo.UpdateLoginUser(
				user.UserId,
				It.IsAny<DateTime>(),
				It.IsAny<string>(),
				It.IsAny<DateTime>()))
			.ReturnsAsync(loginRequest);

			// Act
			Result<string> result = await _userService.LoginUser(loginParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.AreEqual(user.LastToken, result.Data);
		}

		[TestMethod]
		public async Task LoginUser_Fail_InvalidPassword()
		{
			// Arrange
			LoginParametrs loginParam = new LoginParametrs("user", "Some2323Password");

			Result<UserDTO> repoResult = new Result<UserDTO>();
			UserDTO user = new UserDTO(
				1, "user", "asasassa", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true
				);
			repoResult.Data = user;
			_mockUsersRepository.Setup(repo => repo.GetUserByName(loginParam.Name))
								.ReturnsAsync(repoResult);

			// Act
			Result<string> result = await _userService.LoginUser(loginParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("invalid password", result.ErrorMessage);
		}
	}
}

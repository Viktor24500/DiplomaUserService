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

namespace SystemUserServiceUnitTests.Users
{
	[TestClass]
	public class GetUserTests
	{
		private Mock<IUsersRepository> _mockUsersRepository;
		private Mock<ILogger<UserService>> _mockLogger;
		private UserService _userService;
		private PasswordChecks _passwordChecks;
		private EmailValidation _emailValidation;

		[TestInitialize]
		public void Setup()
		{
			_mockUsersRepository = new Mock<IUsersRepository>();
			_mockLogger = new Mock<ILogger<UserService>>();

			// Dependencies that are not actively tested here can be mocked/null for simplicity
			var mockConfiguration = new Mock<IConfiguration>();

			_userService = new UserService(
				_mockUsersRepository.Object,
				_mockLogger.Object,
				_passwordChecks,
				_emailValidation,
				mockConfiguration.Object);
		}
		[TestMethod]
		public async Task GetAllUsersAsync_Success()
		{
			// Arrange
			List<UserDTO> userDTOs = new List<UserDTO>();
			UserDTO userDTO1 = new UserDTO(
				1, "john.doe", "SecureP@ss123", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true);
			UserDTO userDTO2 = new UserDTO(
				2, "john.doe1", "SecureP@ss523", "john.doe1@example.com",
				"John", "Doe", null, DateTime.UtcNow, DateTime.UtcNow.AddHours(-2),
				"xyz9817abc", DateTime.UtcNow.AddDays(2), false);
			userDTOs.Add(userDTO1);
			userDTOs.Add(userDTO2);

			Result<List<UserDTO>> repoResult = new Result<List<UserDTO>>();
			repoResult.Data = userDTOs;
			repoResult.ErrorCode = 0;
			_mockUsersRepository
				.Setup(repo => repo.GetAllUsers())
				.ReturnsAsync(repoResult);

			// Act
			Result<List<User>> result = await _userService.GetAllUsers();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.IsTrue(result.Data.Count == 2);
			Assert.AreEqual("john.doe", result.Data[0].Username);
			Assert.AreEqual("john.doe1", result.Data[1].Username);
		}
		[TestMethod]
		public async Task GetUserByIdAsync_Success()
		{
			// Arrange
			int userId = 1;
			UserDTO userDTO1 = new UserDTO(
				1, "john.doe", "SecureP@ss123", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true);

			Result<UserDTO> repoResult = new Result<UserDTO>();
			repoResult.Data = userDTO1;
			repoResult.ErrorCode = 0;
			_mockUsersRepository
				.Setup(repo => repo.GetUser(1))
				.ReturnsAsync(repoResult);

			// Act
			Result<User> result = await _userService.GetUser(1);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.AreEqual(userId, result.Data.UserId);
			Assert.AreEqual("john.doe", result.Data.Username);
		}

		[TestMethod]
		public async Task GetUserByIdAsync_Fail_NegativeId()
		{
			// Arrange
			int invalidId = -1;

			// Act
			Result<User> result = await _userService.GetUser(invalidId);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("id can't be negative", result.ErrorMessage);
		}

		[TestMethod]
		public async Task GetUserByIdAsync_Fail_NotFound()
		{
			// Arrange
			int id = 10;
			Result<UserDTO> repoResult = new Result<UserDTO>
			{
				ErrorCode = (int)ErrorCodes.NotFound
			};
			_mockUsersRepository.Setup(repo => repo.GetUser(id)).ReturnsAsync(repoResult);


			// Act
			Result<User> result = await _userService.GetUser(id);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.NotFound, result.ErrorCode);
			Assert.AreEqual("User with 10 not exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task GetUserByActiveStatusAsync_Success()
		{
			// Arrange
			bool isUserActive = true;
			List<UserDTO> userDTOs = new List<UserDTO>();
			UserDTO userDTO1 = new UserDTO(
				1, "john.doe", "SecureP@ss123", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true);
			UserDTO userDTO2 = new UserDTO(
				2, "john.doe1", "SecureP@ss523", "john.doe1@example.com",
				"John", "Doe", null, DateTime.UtcNow, DateTime.UtcNow.AddHours(-2),
				"xyz9817abc", DateTime.UtcNow.AddDays(2), true);
			//UserDTO userDTO3 = new UserDTO(
			//	3, "john.doe2", "SecureP@ss523", "john.doe2@example.com",
			//	"John", "Doe", null, DateTime.UtcNow, DateTime.UtcNow.AddHours(-34),
			//	"xyz34343417abc", DateTime.UtcNow.AddDays(34), false);
			userDTOs.Add(userDTO1);
			userDTOs.Add(userDTO2);
			//userDTOs.Add(userDTO3);

			//User user3 = new User(
			//	3, "john.doe2", "SecureP@ss523", "john.doe2@example.com",
			//	"John", "Doe", null, DateTime.UtcNow, DateTime.UtcNow.AddHours(-34),
			//	DateTime.UtcNow.AddDays(34), false, "xyz34343417abc"
			//	);

			Result<List<UserDTO>> repoResult = new Result<List<UserDTO>>();
			repoResult.Data = userDTOs;
			repoResult.ErrorCode = 0;
			_mockUsersRepository
				.Setup(repo => repo.GetUserByActiveStatus(isUserActive))
				.ReturnsAsync(repoResult);

			// Act
			Result<List<User>> result = await _userService.GetUserByActiveStatus(isUserActive);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.IsTrue(result.Data.Count == 2);

			Assert.AreEqual("john.doe", result.Data[0].Username);
			Assert.AreEqual("john.doe1", result.Data[1].Username);

			Assert.IsTrue(result.Data[0].IsActive);
			Assert.IsTrue(result.Data[1].IsActive);

			//Assert.IsFalse(result.Data.Contains(user3));
		}

		[TestMethod]
		public async Task GetUserByName_Success()
		{
			// Arrange
			string userName = "john.doe";
			UserDTO userDTO = new UserDTO(
				1, "john.doe", "SecureP@ss123", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true);

			Result<UserDTO> repoResult = new Result<UserDTO>
			{
				Data = userDTO,
				ErrorCode = 0
			};

			_mockUsersRepository
				.Setup(repo => repo.GetUserByName(userName))
				.ReturnsAsync(repoResult);

			// Act
			Result<User> result = await _userService.GetUserByName(userName);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.AreEqual(userDTO.UserId, result.Data.UserId);
			Assert.AreEqual(userName, result.Data.Username);
		}

		[TestMethod]
		public async Task GetUserByName_UserNotFound()
		{
			// Arrange
			string userName = "unknown.user";
			Result<UserDTO> repoResult = new Result<UserDTO>
			{
				ErrorCode = (int)ErrorCodes.NotFound,
				ErrorMessage = "User not found"
			};

			_mockUsersRepository
				.Setup(repo => repo.GetUserByName(userName))
				.ReturnsAsync(repoResult);

			// Act
			Result<User> result = await _userService.GetUserByName(userName);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.NotFound, result.ErrorCode);
			Assert.AreEqual($"User with {userName} not exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task GetUserByName_NameIsNullOrEmpty()
		{
			// Arrange
			string userName = null;

			// Act
			Result<User> result = await _userService.GetUserByName(userName);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("name can't be null or empty", result.ErrorMessage);
		}
	}
}

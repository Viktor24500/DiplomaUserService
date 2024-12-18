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
	public class UpdateUserTests
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
		public async Task UpdateUser_Fail_IdIsNegative()
		{
			// Arrange
			UserUpdateParameters updateParameters = new UserUpdateParameters(-1, "john.doe@example.com", "John", "Doe", null, true);

			// Act
			Result<User> result = await _userService.UpdateUser(updateParameters);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("id can't be negative", result.ErrorMessage);
		}

		[TestMethod]
		public async Task UpdateUser_Fail_EmailIsEmpty()
		{
			// Arrange
			UserUpdateParameters updateParameters = new UserUpdateParameters(1, "", "John", "Doe", null, true);

			// Act
			Result<User> result = await _userService.UpdateUser(updateParameters);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("email can't be null or empty", result.ErrorMessage);
		}

		[TestMethod]
		public async Task UpdateUser_Fail_FirstOrLastNameIsEmpty()
		{
			// Arrange
			UserUpdateParameters updateParameters = new UserUpdateParameters(1, "john.doe@example.com", "", "Doe", null, true);

			// Act
			Result<User> result = await _userService.UpdateUser(updateParameters);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("firstName or lastName can't be null or empty", result.ErrorMessage);
		}

		[TestMethod]
		public async Task UpdateUser_Fail_EmailPatternInvalid()
		{
			// Arrange
			UserUpdateParameters updateParameters = new UserUpdateParameters(1, "invalid-email", "John", "Doe", null, true);

			// Act
			Result<User> result = await _userService.UpdateUser(updateParameters);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("email not match with pattern", result.ErrorMessage);
		}

		[TestMethod]
		public async Task UpdateUser_Fail_EmailConflict()
		{
			// Arrange
			UserUpdateParameters updateParameters = new UserUpdateParameters(1, "existing@example.com", "John", "Doe", null, true);

			UserDTO existingUser = new UserDTO(2, "jane.doe", "asasassa", "existing@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true);
			Result<UserDTO> repositoryResult = new Result<UserDTO>();
			repositoryResult.Data = existingUser;

			_mockUsersRepository.Setup(repo => repo.GetUserByEmail(updateParameters.Email))
				.ReturnsAsync(repositoryResult);

			// Act
			Result<User> result = await _userService.UpdateUser(updateParameters);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Conflict, result.ErrorCode);
			Assert.AreEqual($"User with email {updateParameters.Email} exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task UpdateUser_Success()
		{
			// Arrange
			UserUpdateParameters updateParameters = new UserUpdateParameters(1, "john.doe@example.com", "John", "Doe", "Michael", true);

			Result repositoryUpdateResult = new Result();
			repositoryUpdateResult.ErrorCode = (int)ErrorCodes.Success;
			UserDTO updatedUser = new UserDTO(
				1, "user", "asasassa", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true);
			Result<UserDTO> repositoryGetResult = new Result<UserDTO>();
			repositoryGetResult.Data = updatedUser;

			Result<UserDTO> getUserByEmail = new Result<UserDTO>();
			getUserByEmail.ErrorCode = (int)ErrorCodes.NotFound;

			_mockUsersRepository.Setup(repo => repo.UpdateUser(updateParameters.Id, updateParameters.Email, updateParameters.FirstName, updateParameters.LastName, updateParameters.FatherName, updateParameters.IsActive))
				.ReturnsAsync(repositoryUpdateResult);

			_mockUsersRepository.Setup(repo => repo.GetUser(updateParameters.Id))
				.ReturnsAsync(repositoryGetResult);

			_mockUsersRepository.Setup(repo => repo.GetUserByEmail(updateParameters.Email))
				.ReturnsAsync(getUserByEmail);

			// Act
			Result<User> result = await _userService.UpdateUser(updateParameters);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.IsNotNull(result.Data);
			Assert.AreEqual(updateParameters.Id, result.Data.UserId);
			Assert.AreEqual(updateParameters.Email, result.Data.Email);
			Assert.AreEqual(updateParameters.FirstName, result.Data.FirstName);
			Assert.AreEqual(updateParameters.LastName, result.Data.LastName);
			Assert.AreEqual(updateParameters.FatherName, result.Data.FatherName);
			Assert.AreEqual(updateParameters.IsActive, result.Data.IsActive);
		}
	}

}

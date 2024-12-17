using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.BusinessLogic.Services;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.UsersRoles;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace UserServiceTest.UserRoles
{
	[TestClass]
	public class GetUserRoleTests
	{
		private Mock<IUserRoleRepository> _mockUserRoleRepository;
		private Mock<IUsersRepository> _mockUserRepository;
		private Mock<IRolesRepository> _mockRoleRepository;
		private Mock<ILogger<UserRoleService>> _mockLogger;
		private UserRoleService _userRoleService;

		[TestInitialize]
		public void Setup()
		{
			_mockUserRoleRepository = new Mock<IUserRoleRepository>();
			_mockUserRepository = new Mock<IUsersRepository>();
			_mockRoleRepository = new Mock<IRolesRepository>();
			_mockLogger = new Mock<ILogger<UserRoleService>>();

			_userRoleService = new UserRoleService(
				_mockUserRoleRepository.Object,
				_mockLogger.Object,
				_mockRoleRepository.Object,
				_mockUserRepository.Object);
		}

		[TestMethod]
		public async Task GetAllUserRolesAsync_Success()
		{
			//Arrange
			List<UserRoleDTO> userRoleDTOs = new List<UserRoleDTO>();

			UserRoleDTO userRoleDTO1 = new UserRoleDTO(
				1, 1, 1,
				1, "Admin", "Administrator role with full access",
				1, "john.doe", "SecureP@ss123",
				"john.doe@example.com", "John", "Doe",
				"Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				DateTime.UtcNow.AddDays(1), "xyz98327abc",
				true
			);

			UserRoleDTO userRoleDTO2 = new UserRoleDTO(
				1, 3, 1,
				1, "Admin", "Administrator role with full access",
				1, "john.doe", "SecureP@ss12121213",
				"john.doe@example.com", "John", "Doe",
				null, DateTime.UtcNow, DateTime.UtcNow.AddHours(-23),
				DateTime.UtcNow.AddDays(11), "xyz983wss27abc",
				true
			);
			UserRoleDTO userRoleDTO3 = new UserRoleDTO(
				1, 2, 3,
				1, "Admin", "Administrator role with full access",
				1, "john.doe", "SecureP@ss121asa13",
				"john.doe@example.com", "John", "Doe",
				null, DateTime.UtcNow, DateTime.UtcNow.AddHours(-23),
				DateTime.UtcNow.AddDays(11), "xyz983wss27abc",
				false
			);

			userRoleDTOs.Add(userRoleDTO1);
			userRoleDTOs.Add(userRoleDTO2);
			userRoleDTOs.Add(userRoleDTO3);

			Result<List<UserRoleDTO>> repoResult = new Result<List<UserRoleDTO>>();
			repoResult.Data = userRoleDTOs;
			repoResult.ErrorCode = 0;
			_mockUserRoleRepository
				.Setup(repo => repo.GetAllUsersRoles())
				.ReturnsAsync(repoResult);

			// Act
			Result<List<UserRole>> result = await _userRoleService.GetAllUsersRoles();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.IsTrue(result.Data.Count == 3);
			Assert.AreEqual("john.doe", result.Data[0].User.Username);
			Assert.AreEqual("Admin", result.Data[1].Role.Name);
			Assert.AreEqual(true, result.Data[1].User.IsActive);
		}

		[TestMethod]
		public async Task GetAllUserRolesByUserIdAsync_Success()
		{
			//Arrange
			int userId = 1;

			UserRoleDTO userRoleDTO1 = new UserRoleDTO(
				1, 1, 1,
				1, "Admin", "Administrator role with full access",
				1, "john.doe", "SecureP@ss123",
				"john.doe@example.com", "John", "Doe",
				"Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				DateTime.UtcNow.AddDays(1), "xyz98327abc",
				true
			);

			Result<UserRoleDTO> repoResult = new Result<UserRoleDTO>();
			repoResult.Data = userRoleDTO1;
			repoResult.ErrorCode = 0;
			_mockUserRoleRepository
				.Setup(repo => repo.GetUserRoleByUserId(userId))
				.ReturnsAsync(repoResult);

			// Act
			Result<UserRole> result = await _userRoleService.GetUserRoleByUserId(userId);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.AreEqual(1, result.Data.UserRolesUserId);
			Assert.AreEqual("Admin", result.Data.Role.Name);
		}

		[TestMethod]
		public async Task GetAllUserRolesByUserIdAsync_Fail_NegativeId()
		{
			//Arrange
			int userId = -1;

			// Act
			Result<UserRole> result = await _userRoleService.GetUserRoleByUserId(userId);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("id can't be negative", result.ErrorMessage);
		}

		[TestMethod]
		public async Task GetAllUserRolesByUserIdAsync_Fail_NotFound()
		{
			//Arrange
			int id = 10;
			Result<UserRoleDTO> repoResult = new Result<UserRoleDTO>();
			repoResult.ErrorCode = (int)ErrorCodes.NotFound;
			_mockUserRoleRepository.Setup(repo => repo.GetUserRoleByUserId(id)).ReturnsAsync(repoResult);


			// Act
			Result<UserRole> result = await _userRoleService.GetUserRoleByUserId(id);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.NotFound, result.ErrorCode);
			Assert.AreEqual("User role with user id 10 not exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task GetAllUserRolesByRoleIdAsync_Success()
		{
			//Arrange
			int roleId = 1;

			//Arrange
			List<UserRoleDTO> userRoleDTOs = new List<UserRoleDTO>();

			UserRoleDTO userRoleDTO1 = new UserRoleDTO(
				1, 1, 1,
				1, "Admin", "Administrator role with full access",
				1, "john.doe", "SecureP@ss123",
				"john.doe@example.com", "John", "Doe",
				"Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				DateTime.UtcNow.AddDays(1), "xyz98327abc",
				true
			);

			UserRoleDTO userRoleDTO2 = new UserRoleDTO(
				1, 1, 1,
				1, "Admin", "Administrator role with full access",
				1, "john.doe", "SecureP@ss12121213",
				"john.doe@example.com", "John", "Doe",
				null, DateTime.UtcNow, DateTime.UtcNow.AddHours(-23),
				DateTime.UtcNow.AddDays(11), "xyz983wss27abc",
				true
			);

			userRoleDTOs.Add(userRoleDTO1);
			userRoleDTOs.Add(userRoleDTO2);

			Result<List<UserRoleDTO>> repoResult = new Result<List<UserRoleDTO>>();
			repoResult.Data = userRoleDTOs;
			repoResult.ErrorCode = 0;
			_mockUserRoleRepository
				.Setup(repo => repo.GetUserRoleByRoleId(roleId))
				.ReturnsAsync(repoResult);

			// Act
			Result<List<UserRole>> result = await _userRoleService.GetUserRoleByRoleId(roleId);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.AreEqual(1, result.Data[0].UserRolesRoleId);
			Assert.AreEqual(1, result.Data[1].UserRolesRoleId);
			Assert.AreEqual("Admin", result.Data[0].Role.Name);
		}

		[TestMethod]
		public async Task GetAllUserRolesByRoleIdAsync_Fail_NegativeId()
		{
			//Arrange
			int userId = -1;

			// Act
			Result<List<UserRole>> result = await _userRoleService.GetUserRoleByRoleId(userId);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("id can't be negative", result.ErrorMessage);
		}

		[TestMethod]
		public async Task GetAllUserRolesByRoleIdAsync_Fail_NotFound()
		{
			//Arrange
			int id = 10;
			Result<List<UserRoleDTO>> repoResult = new Result<List<UserRoleDTO>>
			{
				ErrorCode = (int)ErrorCodes.NotFound
			};
			_mockUserRoleRepository.Setup(repo => repo.GetUserRoleByRoleId(id)).ReturnsAsync(repoResult);


			// Act
			Result<List<UserRole>> result = await _userRoleService.GetUserRoleByRoleId(id);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.NotFound, result.ErrorCode);
			Assert.AreEqual("User role with role id 10 not exist", result.ErrorMessage);
		}
	}
}

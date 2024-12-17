using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.BusinessLogic.Parametrs.UserRole;
using SystemUserService.BusinessLogic.Services;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Roles;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.DTO.UsersRoles;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace UserServiceTest.UserRoles
{
	[TestClass]
	public class CreateUserRoleTest
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
		public async Task CreateUserRolesAsync_Fail_UserNegativeId()
		{
			//Arrange
			UserRoleCreateParameters createParam = new UserRoleCreateParameters(-1, 1);

			// Act
			Result<UserRole> result = await _userRoleService.CreateUserRole(createParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("user id can't be negative", result.ErrorMessage);
		}


		[TestMethod]
		public async Task CreateUserRolesAsync_Fail_RoleNegativeId()
		{
			//Arrange
			UserRoleCreateParameters createParam = new UserRoleCreateParameters(1, -1);

			UserDTO userDTO = new UserDTO(
				1, "john.doe", "SecureP@ss123", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true);
			Result<UserDTO> userRes = new Result<UserDTO>();
			userRes.Data = userDTO;

			_mockUserRepository.Setup(user => user.GetUser(createParam.UserId)).ReturnsAsync(userRes);
			// Act
			Result<UserRole> result = await _userRoleService.CreateUserRole(createParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("role id can't be negative", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUserRolesAsync_Fail_RoleNotFound()
		{
			//Arrange
			UserRoleCreateParameters createParam = new UserRoleCreateParameters(1, 10);
			Result<RoleDTO> roleRes = new Result<RoleDTO>();
			roleRes.ErrorCode = (int)ErrorCodes.NotFound;

			UserDTO userDTO = new UserDTO(
			1, "john.doe", "SecureP@ss123", "john.doe@example.com",
			"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
			"xyz98327abc", DateTime.UtcNow.AddDays(1), true);
			Result<UserDTO> userRes = new Result<UserDTO>();
			userRes.Data = userDTO;

			_mockUserRepository.Setup(user => user.GetUser(createParam.UserId)).ReturnsAsync(userRes);
			_mockRoleRepository.Setup(role => role.GetRole(createParam.RoleId)).ReturnsAsync(roleRes);

			// Act
			Result<UserRole> result = await _userRoleService.CreateUserRole(createParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.NotFound, result.ErrorCode);
			Assert.AreEqual("Role with id 10 not exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUserRolesAsync_Fail_UserNotFound()
		{
			//Arrange
			UserRoleCreateParameters createParam = new UserRoleCreateParameters(10, 1);
			Result<UserDTO> userRes = new Result<UserDTO>();
			userRes.ErrorCode = (int)ErrorCodes.NotFound;

			RoleDTO roleDTO = new RoleDTO(1, "Admin", null);
			Result<RoleDTO> roleRes = new Result<RoleDTO>();
			roleRes.Data = roleDTO;

			_mockRoleRepository.Setup(role => role.GetRole(createParam.RoleId)).ReturnsAsync(roleRes);
			_mockUserRepository.Setup(user => user.GetUser(createParam.UserId)).ReturnsAsync(userRes);

			// Act
			Result<UserRole> result = await _userRoleService.CreateUserRole(createParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.NotFound, result.ErrorCode);
			Assert.AreEqual("user with id 10 not exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateUserRolesAsync_Success()
		{
			//Arrange
			UserRoleCreateParameters createParam = new UserRoleCreateParameters(1, 1);
			UserDTO userDTO = new UserDTO(
				1, "john.doe", "SecureP@ss123", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				"xyz98327abc", DateTime.UtcNow.AddDays(1), true);
			Result<UserDTO> userRes = new Result<UserDTO>();
			userRes.Data = userDTO;

			RoleDTO roleDTO = new RoleDTO(1, "Admin", null);
			Result<RoleDTO> roleRes = new Result<RoleDTO>();
			roleRes.Data = roleDTO;

			UserRoleDTO userRole = new UserRoleDTO(
				1, 1, 1,
				1, "Admin", null,
				1, "john.doe", "SecureP@ss123", "john.doe@example.com",
				"John", "Doe", "Michael", DateTime.UtcNow, DateTime.UtcNow.AddHours(-1),
				DateTime.UtcNow.AddDays(1), "xyz98327abc", true);
			Result<UserRoleDTO> userRoleRes = new Result<UserRoleDTO>();
			userRoleRes.Data = userRole;

			Result createUserRoleRes = new Result();

			_mockUserRepository.Setup(user => user.GetUser(createParam.UserId)).ReturnsAsync(userRes);
			_mockRoleRepository.Setup(role => role.GetRole(createParam.RoleId)).ReturnsAsync(roleRes);
			_mockUserRoleRepository.Setup(userRole => userRole.CreateUserRole(createParam.UserId, createParam.RoleId)).ReturnsAsync(createUserRoleRes);
			_mockUserRoleRepository.Setup(userRole => userRole.GetUserRoleByUserId(createParam.UserId)).ReturnsAsync(userRoleRes);

			// Act
			Result<UserRole> result = await _userRoleService.CreateUserRole(createParam);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.AreEqual(1, result.Data.UserRolesUserId);
			Assert.AreEqual(1, result.Data.UserRolesRoleId);
		}
	}
}

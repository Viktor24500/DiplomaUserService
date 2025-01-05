using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Services;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Roles;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace UserServiceTest.Roles
{
	[TestClass]
	internal class GetRoleTests
	{
		private Mock<IRolesRepository> _mockRolesRepository;
		private Mock<ILogger<RoleService>> _mockLogger;
		private RoleService _roleService;

		[TestInitialize]
		public void Setup()
		{
			_mockRolesRepository = new Mock<IRolesRepository>();
			_mockLogger = new Mock<ILogger<RoleService>>();

			_roleService = new RoleService(
				_mockRolesRepository.Object,
				_mockLogger.Object);
		}

		[TestMethod]
		public async void GetAllRolesAsync_Success()
		{
			// Arrange
			List<RoleDTO> roleDTOs = new List<RoleDTO>();
			RoleDTO roleDTO1 = new RoleDTO(2, "User", "Standard user role");
			RoleDTO roleDTO2 = new RoleDTO(2, "User", "Standard user role");
			roleDTOs.Add(roleDTO1);
			roleDTOs.Add(roleDTO2);

			Result<List<RoleDTO>> repoResult = new Result<List<RoleDTO>>();
			repoResult.Data = roleDTOs;
			repoResult.ErrorCode = (int)ErrorCodes.Success;

			_mockRolesRepository
				.Setup(repo => repo.GetAllRoles())
				.ReturnsAsync(repoResult);

			// Act
			Result<List<Role>> result = await _roleService.GetAllRoles();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.IsTrue(result.Data.Count == 2);
			Assert.AreEqual("Admin", result.Data[0].Name);
			Assert.AreEqual("User", result.Data[1].Name);
		}

		[TestMethod]
		public async void GetRoleAsync_Success()
		{
			// Arrange
			int roleId = 1;
			RoleDTO roleDTO = new RoleDTO(roleId, "Admin", "Administrator role");

			Result<RoleDTO> repoResult = new Result<RoleDTO>();
			repoResult.Data = roleDTO;
			repoResult.ErrorCode = (int)ErrorCodes.Success;

			_mockRolesRepository
				.Setup(repo => repo.GetRole(roleId))
				.ReturnsAsync(repoResult);

			// Act
			Result<Role> result = await _roleService.GetRole(roleId);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.IsNotNull(result.Data);
			Assert.AreEqual(roleId, result.Data.Id);
			Assert.AreEqual("Admin", result.Data.Name);
			Assert.AreEqual("Administrator role", result.Data.Description);
		}

		[TestMethod]
		public async Task GetRoleAsync_Fail_NegativeId()
		{
			// Arrange
			int roleId = -1;

			// Act
			Result<Role> result = await _roleService.GetRole(roleId);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("id can't be negative", result.ErrorMessage);
		}

		[TestMethod]
		public async Task GetRoleAsync_Fail_NotFound()
		{
			// Arrange
			int roleId = 100;
			Result<RoleDTO> repoResult = new Result<RoleDTO>();
			repoResult.ErrorCode = (int)ErrorCodes.NotFound;

			_mockRolesRepository
				.Setup(repo => repo.GetRole(roleId))
				.ReturnsAsync(repoResult);

			// Act
			Result<Role> result = await _roleService.GetRole(roleId);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.NotFound, result.ErrorCode);
			Assert.AreEqual($"Role with {roleId} not exist", result.ErrorMessage);
		}
	}
}

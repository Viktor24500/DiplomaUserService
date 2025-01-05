using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Parametrs.Roles;
using SystemUserService.BusinessLogic.Services;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Roles;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace UserServiceTest.Roles
{
	[TestClass]
	public class UpdateRoleTests
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
		public async Task UpdateRole_Fail_NameNullOrEmpty()
		{
			//Arrange
			RoleUpdateParametrs updateParam = new RoleUpdateParametrs(1, "", null);

			//Act
			Result<Role> result = await _roleService.UpdateRole(updateParam);

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("name can't be null or empty", result.ErrorMessage);
		}

		[TestMethod]
		public async Task UpdateRole_Fail_Conflict()
		{
			//Arrange
			RoleUpdateParametrs updateParam = new RoleUpdateParametrs(1, "Admin", null);
			RoleDTO roleDTO = new RoleDTO(2, "Admin", "Administrator role");

			Result<RoleDTO> expectedResult = new Result<RoleDTO>();
			expectedResult.Data = roleDTO;
			expectedResult.ErrorCode = (int)ErrorCodes.Conflict;

			_mockRolesRepository
				.Setup(repo => repo.GetRoleByName(updateParam.Name))
				.ReturnsAsync(expectedResult);

			//Act
			Result<Role> result = await _roleService.UpdateRole(updateParam);

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Conflict, result.ErrorCode);
			Assert.AreEqual($"Role with name {updateParam.Name} exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task UpdateRole_Success()
		{
			//Arrange
			RoleUpdateParametrs updateParam = new RoleUpdateParametrs(1, "test", "Administrator role");
			Result expectedResult = new Result();
			expectedResult.ErrorCode = (int)ErrorCodes.Success;

			RoleDTO roleDTO = new RoleDTO(1, "test", "Administrator role");

			Result<RoleDTO> getRoleByNameResult = new Result<RoleDTO>();
			getRoleByNameResult.Data = roleDTO;
			getRoleByNameResult.ErrorCode = (int)ErrorCodes.NotFound;

			Result<RoleDTO> repoResult = new Result<RoleDTO>();
			repoResult.Data = roleDTO;
			repoResult.ErrorCode = (int)ErrorCodes.Success;

			_mockRolesRepository
				.Setup(repo => repo.UpdateRole(updateParam.Id, updateParam.Name, updateParam.Description))
				.ReturnsAsync(expectedResult);

			_mockRolesRepository
				.Setup(repo => repo.GetRoleByName(updateParam.Name))
				.ReturnsAsync(getRoleByNameResult);

			_mockRolesRepository
				.Setup(repo => repo.GetRole(updateParam.Id))
				.ReturnsAsync(repoResult);

			//Act
			Result<Role> result = await _roleService.UpdateRole(updateParam);

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.AreEqual(updateParam.Name, result.Data.Name);
		}
	}
}

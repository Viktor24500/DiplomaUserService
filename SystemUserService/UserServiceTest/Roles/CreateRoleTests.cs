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
	public class CreateRoleTests
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
		public async Task CreateRole_Fail_NameNullOrEmpty()
		{
			//Arrange
			RoleCreateParametrs createParam = new RoleCreateParametrs("", null);

			//Act
			Result<Role> result = await _roleService.CreateRole(createParam);

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.BadRequest, result.ErrorCode);
			Assert.AreEqual("name can't be null or empty", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateRole_Fail_Conflict()
		{
			//Arrange
			RoleCreateParametrs createParam = new RoleCreateParametrs("test", null);
			Result<int> expectedResult = new Result<int>();
			expectedResult.ErrorCode = (int)ErrorCodes.Conflict;

			Result<RoleDTO> getRoleByNameResult = new Result<RoleDTO>();
			getRoleByNameResult.ErrorCode = (int)ErrorCodes.Success;

			_mockRolesRepository
				.Setup(repo => repo.GetRoleByName(createParam.Name))
				.ReturnsAsync(getRoleByNameResult);

			_mockRolesRepository
				.Setup(repo => repo.CreateRole(createParam.Name, createParam.Description))
				.ReturnsAsync(expectedResult);

			//Act
			Result<Role> result = await _roleService.CreateRole(createParam);

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Conflict, result.ErrorCode);
			Assert.AreEqual($"Role with name {createParam.Name} exist", result.ErrorMessage);
		}

		[TestMethod]
		public async Task CreateRole_Success()
		{
			//Arrange
			RoleCreateParametrs createParam = new RoleCreateParametrs("Admin", null);
			Result<int> expectedResult = new Result<int>();
			expectedResult.Data = 1;

			RoleDTO roleDTO = new RoleDTO(1, "Admin", "Administrator role");

			Result<RoleDTO> repoResult = new Result<RoleDTO>();
			repoResult.Data = roleDTO;
			repoResult.ErrorCode = (int)ErrorCodes.Success;

			Result<RoleDTO> getRoleByNameResult = new Result<RoleDTO>();
			getRoleByNameResult.ErrorCode = (int)ErrorCodes.NotFound;

			_mockRolesRepository
				.Setup(repo => repo.GetRoleByName(createParam.Name))
				.ReturnsAsync(getRoleByNameResult);

			_mockRolesRepository
				.Setup(repo => repo.CreateRole(createParam.Name, createParam.Description))
				.ReturnsAsync(expectedResult);

			_mockRolesRepository
				.Setup(repo => repo.GetRole(expectedResult.Data))
				.ReturnsAsync(repoResult);

			//Act
			Result<Role> result = await _roleService.CreateRole(createParam);

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)ErrorCodes.Success, result.ErrorCode);
			Assert.AreEqual(createParam.Name, result.Data.Name);
		}
	}
}

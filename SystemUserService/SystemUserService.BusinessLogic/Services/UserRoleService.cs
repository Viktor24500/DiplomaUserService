using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.BusinessLogic.Extensions;
using SystemUserService.BusinessLogic.Parametrs.UserRole;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Roles;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.DTO.UsersRoles;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.BusinessLogic.Services
{
	public class UserRoleService : IUserRoleService
	{
		private IUserRoleRepository _userRolesRepository;
		private IUsersRepository _usersRepository;
		private IRolesRepository _rolesRepository;
		private ILogger<UserRoleService> _logger;
		public UserRoleService(IUserRoleRepository userRolesRepository, ILogger<UserRoleService> logger,
			IRolesRepository rolesRepository, IUsersRepository usersRepository)
		{
			_userRolesRepository = userRolesRepository;
			_logger = logger;
			_rolesRepository = rolesRepository;
			_usersRepository = usersRepository;
		}

		public async Task<Result<UserRole>> CreateUserRole(UserRoleCreateParameters userRoleCreateParam)
		{
			//Validate user id
			Result<UserRole> result = new Result<UserRole>();
			if (IntExtension.IsNegative(userRoleCreateParam.UserId))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "user id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<UserDTO> repUserResult = await _usersRepository.GetUser(userRoleCreateParam.UserId);
			if (repUserResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				_logger.LogError(repUserResult.ErrorMessage);
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"user with id {userRoleCreateParam.UserId} not exist";
				return result;
			}

			//Validate role id
			if (IntExtension.IsNegative(userRoleCreateParam.RoleId))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "role id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			Result<RoleDTO> repRoleResult = new Result<RoleDTO>();
			repRoleResult = await _rolesRepository.GetRole(userRoleCreateParam.RoleId);
			if (repRoleResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				_logger.LogError(repRoleResult.ErrorMessage);
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"role with id {userRoleCreateParam.RoleId} not exist";
				return result;
			}
			Result repUserRoleResult = await _userRolesRepository.CreateUserRole(userRoleCreateParam.UserId,
				userRoleCreateParam.RoleId);
			if (repUserRoleResult.ErrorCode == (int)ErrorCodes.Success)
			{
				Result<UserRoleDTO> repReult = await _userRolesRepository.GetUserRoleByUserId(userRoleCreateParam.UserId);
				result.Data = repReult.Data.MapToUserRole();
				return result;
			}
			result.ErrorCode = repUserRoleResult.ErrorCode;
			result.ErrorMessage = repUserRoleResult.ErrorMessage;
			_logger.LogError(result.ErrorMessage);
			return result;
		}

		public async Task<Result<List<UserRole>>> GetAllUsersRoles()
		{
			Result<List<UserRoleDTO>> repResult = await _userRolesRepository.GetAllUsersRoles();
			Result<List<UserRole>> result = new Result<List<UserRole>>();
			result.Data = repResult.Data.MapToUserRoleCollection();
			return result;
		}

		public async Task<Result<List<UserRole>>> GetUserRoleByRoleId(int id)
		{
			Result<List<UserRole>> result = new Result<List<UserRole>>();
			if (IntExtension.IsNegative(id))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<List<UserRoleDTO>> repResult = await _userRolesRepository.GetUserRoleByRoleId(id);
			if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"User role with role id {id} not exist";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			result.Data = repResult.Data.MapToUserRoleCollection();
			return result;
		}

		public async Task<Result<UserRole>> GetUserRoleByUserId(int id)
		{
			Result<UserRole> result = new Result<UserRole>();
			if (IntExtension.IsNegative(id))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<UserRoleDTO> repResult = await _userRolesRepository.GetUserRoleByUserId(id);
			if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"userRole with user id {id} not exist";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			result.Data = repResult.Data.MapToUserRole();
			return result;
		}

		public async Task<Result<UserRole>> UpdateUserRole(UserRoleUpdateParameters userRoleUpdateParam)
		{
			Result<UserRole> result = new Result<UserRole>();
			if (IntExtension.IsNegative(userRoleUpdateParam.UserRoleId))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "userRoleId can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			Result<UserRoleDTO> checkUserRoleResult = await _userRolesRepository.GetUserRoleByUserRoleId(userRoleUpdateParam.UserRoleId);
			if (checkUserRoleResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				_logger.LogError(checkUserRoleResult.ErrorMessage);
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"userRole with id {userRoleUpdateParam.UserRoleId} not exist";
				return result;
			}

			//Validate user id
			if (IntExtension.IsNegative(userRoleUpdateParam.UserId))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "user id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<UserDTO> repUserResult = await _usersRepository.GetUser(userRoleUpdateParam.UserId);
			if (repUserResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				_logger.LogError(repUserResult.ErrorMessage);
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"user with id {userRoleUpdateParam.UserId} not exist";
				return result;
			}

			//Validate role id

			if (IntExtension.IsNegative(userRoleUpdateParam.RoleId))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "role id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<RoleDTO> repRoleResult = new Result<RoleDTO>();
			repRoleResult = await _rolesRepository.GetRole(userRoleUpdateParam.RoleId);
			if (repRoleResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				_logger.LogError(repRoleResult.ErrorMessage);
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"role with id {userRoleUpdateParam.RoleId} not exist";
				return result;
			}
			Result repUserRoleResult = await _userRolesRepository.UpdateUserRole(userRoleUpdateParam.UserRoleId,
				userRoleUpdateParam.UserId, userRoleUpdateParam.RoleId);
			if (repUserRoleResult.ErrorCode == (int)ErrorCodes.Success)
			{
				Result<UserRoleDTO> repReult = await _userRolesRepository.GetUserRoleByUserId(userRoleUpdateParam.UserId);
				result.Data = repReult.Data.MapToUserRole();
				return result;
			}
			result.ErrorCode = repUserRoleResult.ErrorCode;
			result.ErrorMessage = repUserRoleResult.ErrorMessage;
			_logger.LogError(result.ErrorMessage);
			return result;
		}
	}
}

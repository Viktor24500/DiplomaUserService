using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.BusinessLogic.Extensions;
using SystemUserService.BusinessLogic.Parametrs.RolePermission;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Permissions;
using SystemUserService.DataAccess.DTO.Roles;
using SystemUserService.DataAccess.DTO.RolesPermissions;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.BusinessLogic.Services
{
	public class RolePermissionsService : IRolePermissionsService
	{
		private IRolePermissionsRepository _rolesPermissionsRepository;
		private IPermissionsRepository _permissionsRepository;
		private IRolesRepository _rolesRepository;
		private ILogger<RolePermissionsService> _logger;
		public RolePermissionsService(IRolePermissionsRepository rolesPermissionsRepository, ILogger<RolePermissionsService> logger,
			IRolesRepository rolesRepository, IPermissionsRepository permissionsRepository)
		{
			_rolesPermissionsRepository = rolesPermissionsRepository;
			_logger = logger;
			_rolesRepository = rolesRepository;
			_permissionsRepository = permissionsRepository;
		}
		public async Task<Result<RolePermission>> CreateRolePermissions(RolePermissionCreateParameters rolePermissionCreateParameters)
		{
			//Validate role id
			Result<RolePermission> result = new Result<RolePermission>();
			if (IntExtension.IsNegative(rolePermissionCreateParameters.RoleId))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "role id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<RoleDTO> repRoleResult = await _rolesRepository.GetRole(rolePermissionCreateParameters.RoleId);
			if (repRoleResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"Role with id {rolePermissionCreateParameters.RoleId} not exist";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			//Validate permission id
			foreach (int permissionId in rolePermissionCreateParameters.PermissionsId)
			{
				if (IntExtension.IsNegative(permissionId))
				{
					result.ErrorCode = (int)ErrorCodes.BadRequest;
					result.ErrorMessage = "permission id can't be negative";
					_logger.LogError(result.ErrorMessage);
					return result;
				}
			}
			Result<PermissionDTO> repPermissionResult = new Result<PermissionDTO>();
			foreach (int permissionId in rolePermissionCreateParameters.PermissionsId)
			{
				repPermissionResult = await _permissionsRepository.GetPermission(permissionId);
				if (repPermissionResult.ErrorCode == (int)ErrorCodes.NotFound)
				{
					result.ErrorCode = (int)ErrorCodes.Conflict;
					result.ErrorMessage = $"Permission with id {permissionId} not exist";
					_logger.LogError(result.ErrorMessage);
					return result;
				}
			}
			Result repRolePermissionCreateResult = await _rolesPermissionsRepository.CreateRolePermissions(rolePermissionCreateParameters.RoleId,
				rolePermissionCreateParameters.PermissionsId);
			if (repRolePermissionCreateResult.ErrorCode == (int)ErrorCodes.Success)
			{
				Result<RolePermissionsDTO> repReult = await _rolesPermissionsRepository.GetRolePermissionsByRoleId(rolePermissionCreateParameters.RoleId);
				result.Data = repReult.Data.MapToRolesPermissions();
				return result;
			}
			return result;
		}

		public async Task<Result<List<RolePermission>>> GetAllRolePermissions()
		{
			Result<List<RolePermissionsDTO>> repResult = await _rolesPermissionsRepository.GetAllRolePermissions();
			Result<List<RolePermission>> result = new Result<List<RolePermission>>();
			result.Data = repResult.Data.MapToRolesPermissionsCollection();
			return result;
		}

		public async Task<Result<RolePermission>> GetRolePermissionsByRoleId(int id)
		{
			Result<RolePermission> result = new Result<RolePermission>();
			if (IntExtension.IsNegative(id))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<RoleDTO> repRoleResult = await _rolesRepository.GetRole(id);
			if (repRoleResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"Role with id {id} not exist";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			Result<RolePermissionsDTO> repResult = await _rolesPermissionsRepository.GetRolePermissionsByRoleId(id);
			if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"Role permission with role id  {id} not exist";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			result.Data = repResult.Data.MapToRolesPermissions();
			return result;
		}

		public async Task<Result<RolePermission>> UpdateRolePermissions(RolePermissionUpdateParameters rolePermissionUpdateParameters)
		{
			Result<RolePermission> result = new Result<RolePermission>();
			if (IntExtension.IsNegative(rolePermissionUpdateParameters.RolePermissionId))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "rolePermissionId can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			//Validate role id
			if (IntExtension.IsNegative(rolePermissionUpdateParameters.RoleId))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "role id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<RoleDTO> repRoleResult = await _rolesRepository.GetRole(rolePermissionUpdateParameters.RoleId);
			if (repRoleResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				result.ErrorCode = (int)ErrorCodes.NotFound;
				result.ErrorMessage = $"Role with id {rolePermissionUpdateParameters.RoleId} not exist";
				_logger.LogError(result.ErrorMessage);
				return result;
			}

			//Validate permission id

			if (IntExtension.IsNegative(rolePermissionUpdateParameters.PermissionId))
			{
				result.ErrorCode = (int)ErrorCodes.BadRequest;
				result.ErrorMessage = "permission id can't be negative";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result<PermissionDTO> repPermissionResult = await _permissionsRepository.GetPermission(rolePermissionUpdateParameters.PermissionId);
			if (repPermissionResult.ErrorCode == (int)ErrorCodes.NotFound)
			{
				result.ErrorCode = (int)ErrorCodes.Conflict;
				result.ErrorMessage = $"Permission with id {rolePermissionUpdateParameters.PermissionId} not exist";
				_logger.LogError(result.ErrorMessage);
				return result;
			}
			Result repRolePermissionCreateResult = await _rolesPermissionsRepository.UpdateRolePermissions(rolePermissionUpdateParameters.RolePermissionId,
				rolePermissionUpdateParameters.RoleId, rolePermissionUpdateParameters.PermissionId);
			if (repRolePermissionCreateResult.ErrorCode == (int)ErrorCodes.Success)
			{
				Result<RolePermissionsDTO> repReult = await _rolesPermissionsRepository.GetRolePermissionsByRoleId(rolePermissionUpdateParameters.RoleId);
				result.Data = repReult.Data.MapToRolesPermissions();
				return result;
			}
			return result;
		}
	}
}

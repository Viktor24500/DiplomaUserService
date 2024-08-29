using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.BusinessLogic.Extensions;
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
        public async Task<Result<List<RolesPermission>>> CreateRolePermissions(int roleId, List<int> permissionsId)
        {
            //Validate role id
            Result<List<RolesPermission>> result = new Result<List<RolesPermission>>();
            if (IntExtension.IsNegative(roleId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "role id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<RoleDTO> repRoleResult = await _rolesRepository.GetRole(roleId);
            if (repRoleResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Role with id {roleId} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //Validate permission id
            foreach (int permissionId in permissionsId)
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
            foreach (int permissionId in permissionsId)
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
            Result repRolePermissionCreateResult = await _rolesPermissionsRepository.CreateRolePermissions(roleId, permissionsId);
            if (repRolePermissionCreateResult.ErrorCode == (int)ErrorCodes.Success)
            {
                Result<List<RolePermissionsDTO>> repReult = await _rolesPermissionsRepository.GetRolePermissionsByRoleId(roleId);
                result.Data = repReult.Data.MapToRolesPermissionsCollection();
                return result;
            }
            return result;
        }

        public async Task<Result<List<RolesPermission>>> GetAllRolePermissions()
        {
            Result<List<RolePermissionsDTO>> repResult = await _rolesPermissionsRepository.GetAllRolePermissions();
            Result<List<RolesPermission>> result = new Result<List<RolesPermission>>();
            result.Data = repResult.Data.MapToRolesPermissionsCollection();
            return result;
        }

        public async Task<Result<List<RolesPermission>>> GetRolePermissionsByRoleId(int id)
        {
            Result<List<RolesPermission>> result = new Result<List<RolesPermission>>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<List<RolePermissionsDTO>> repResult = await _rolesPermissionsRepository.GetRolePermissionsByRoleId(id);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Role permission with role id  {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToRolesPermissionsCollection();
            return result;
        }

        public async Task<Result<List<RolesPermission>>> UpdateRolePermissions(int rolePermissionId, int roleId, int permissionId)
        {
            Result<List<RolesPermission>> result = new Result<List<RolesPermission>>();
            if (IntExtension.IsNegative(rolePermissionId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "rolePermissionId can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //Validate role id
            if (IntExtension.IsNegative(roleId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "role id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<RoleDTO> repResult = await _rolesRepository.GetRole(roleId);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Role with id {roleId} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //Validate permission id

            if (IntExtension.IsNegative(permissionId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "permission id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<PermissionDTO> repPermissionResult = new Result<PermissionDTO>();
            repPermissionResult = await _permissionsRepository.GetPermission(permissionId);
            if (repPermissionResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"Permission with id {permissionId} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result repRolePermissionCreateResult = await _rolesPermissionsRepository.UpdateRolePermissions(rolePermissionId, roleId, permissionId);
            if (repRolePermissionCreateResult.ErrorCode == (int)ErrorCodes.Success)
            {
                Result<List<RolePermissionsDTO>> repReult = await _rolesPermissionsRepository.GetRolePermissionsByRoleId(roleId);
                result.Data = repReult.Data.MapToRolesPermissionsCollection();
                return result;
            }
            return result;
        }
    }
}

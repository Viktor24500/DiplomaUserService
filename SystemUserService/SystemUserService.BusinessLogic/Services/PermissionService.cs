using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Extensions;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Permissions;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.BusinessLogic.Services
{
    public class PermissionService : IPermissionService
    {
        private IPermissionsRepository _permissionsRepository;
        private ILogger<PermissionService> _logger;
        public PermissionService(IPermissionsRepository permissionsRepository, ILogger<PermissionService> logger)
        {
            _permissionsRepository = permissionsRepository;
            _logger = logger;
        }

        public async Task<Result<Permission>> CreatePermission(string name, string? description)
        {
            Result<Permission> result = new Result<Permission>();
            if (IsPermissionNameValid(name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<PermissionDTO> repResult = await _permissionsRepository.GetPermissionByName(name);
            if (repResult.ErrorCode == (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"Permission with name {name} exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            repResult = await _permissionsRepository.CreatePermission(name, description);
            result.Data = repResult.Data.MapToPermission();
            return result;
        }

        public async Task<Result<List<Permission>>> GetAllPermissions()
        {
            Result<List<PermissionDTO>> repResult = await _permissionsRepository.GetAllPermissions();
            Result<List<Permission>> result = new Result<List<Permission>>();
            result.Data = repResult.Data.MapToPermissionsCollection();
            return result;
        }

        public async Task<Result<Permission>> GetPermission(int id)
        {
            Result<Permission> result = new Result<Permission>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<PermissionDTO> repResult = await _permissionsRepository.GetPermission(id);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Permission with {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToPermission();
            return result;
        }

        public async Task<Result<Permission>> UpdatePermission(int id, string name, string? description)
        {
            Result<Permission> result = new Result<Permission>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (IsPermissionNameValid(name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<PermissionDTO> repResult = await _permissionsRepository.GetPermissionByName(name);
            if (repResult.ErrorCode != (int)ErrorCodes.NotFound)
            {
                if (repResult.Data.Id != id)
                {
                    result.ErrorCode = (int)ErrorCodes.Conflict;
                    result.ErrorMessage = $"Permission with name {name} exist";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
            }

            repResult = await _permissionsRepository.UpdatePermission(id, name, description);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Permission with {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToPermission();
            return result;
        }

        private bool IsPermissionNameValid(string permissionName)
        {
            return string.IsNullOrWhiteSpace(permissionName);
        }
    }
}

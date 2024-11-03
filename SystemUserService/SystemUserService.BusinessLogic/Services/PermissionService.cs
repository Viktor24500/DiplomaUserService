using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Extensions;
using SystemUserService.BusinessLogic.Parametrs.Permissions;
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

        public async Task<Result<Permission>> CreatePermission(PermissionCreateParametrs createParam)
        {
            Result<Permission> result = new Result<Permission>();
            if (IsPermissionNameValid(createParam.Name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<PermissionDTO> repResult = await _permissionsRepository.GetPermissionByName(createParam.Name);
            if (repResult.ErrorCode == (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"Permission with name {createParam.Name} exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<int> repCreateResult = await _permissionsRepository.CreatePermission(
                createParam.Name, createParam.Description);
            if (repCreateResult.ErrorCode == (int)ErrorCodes.Success)
            {
                repResult = await _permissionsRepository.GetPermission(repCreateResult.Data);
                result.Data = repResult.Data.MapToPermission();
            }
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

        public async Task<Result<Permission>> UpdatePermission(PermissionUpdateParametrs updateParam)
        {
            Result<Permission> result = new Result<Permission>();
            if (IntExtension.IsNegative(updateParam.Id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (IsPermissionNameValid(updateParam.Name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<PermissionDTO> repResult = await _permissionsRepository.GetPermissionByName(updateParam.Name);
            if (repResult.ErrorCode != (int)ErrorCodes.NotFound)
            {
                if (repResult.Data.Id != updateParam.Id)
                {
                    result.ErrorCode = (int)ErrorCodes.Conflict;
                    result.ErrorMessage = $"Permission with name {updateParam.Name} exist";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
            }

            Result repUpdateResult = await _permissionsRepository.UpdatePermission(
                updateParam.Id, updateParam.Name, updateParam.Description);
            if (repUpdateResult.ErrorCode == (int)ErrorCodes.Success)
            {
                repResult = await _permissionsRepository.GetPermission(updateParam.Id);
                if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
                {
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    result.ErrorMessage = $"Permission with {updateParam.Id} not exist";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
                result.Data = repResult.Data.MapToPermission();
                return result;
            }
            return result;
        }

        private bool IsPermissionNameValid(string permissionName)
        {
            return string.IsNullOrWhiteSpace(permissionName);
        }
    }
}

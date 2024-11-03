using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Extensions;
using SystemUserService.BusinessLogic.Parametrs.Roles;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Roles;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.BusinessLogic.Services
{
    public class RoleService : IRoleService
    {
        private IRolesRepository _rolesRepository;
        private ILogger<RoleService> _logger;
        public RoleService(IRolesRepository rolesRepository, ILogger<RoleService> logger)
        {
            _rolesRepository = rolesRepository;
            _logger = logger;
        }

        public async Task<Result<Role>> CreateRole(RoleCreateParametrs createParam)
        {
            Result<Role> result = new Result<Role>();
            if (string.IsNullOrWhiteSpace(createParam.Name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<RoleDTO> repResult = await _rolesRepository.GetRoleByName(createParam.Name);
            if (repResult.ErrorCode == (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"Role with name {createParam.Name} exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<int> repCreateResult = await _rolesRepository.CreateRole(
                createParam.Name, createParam.Description);
            if (repCreateResult.ErrorCode == (int)ErrorCodes.Success)
            {
                repResult = await _rolesRepository.GetRole(repCreateResult.Data);
                result.Data = repResult.Data.MapToRole();
            }
            return result;
        }

        public async Task<Result<List<Role>>> GetAllRoles()
        {
            Result<List<RoleDTO>> repResult = await _rolesRepository.GetAllRoles();
            Result<List<Role>> result = new Result<List<Role>>();
            result.Data = repResult.Data.MapToRolesCollection();
            return result;
        }

        public async Task<Result<Role>> GetRole(int id)
        {
            Result<Role> result = new Result<Role>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<RoleDTO> repResult = await _rolesRepository.GetRole(id);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Role with {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToRole();
            return result;
        }

        public async Task<Result<Role>> UpdateRole(RoleUpdateParametrs updateParam)
        {
            Result<Role> result = new Result<Role>();
            if (IntExtension.IsNegative(updateParam.Id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (string.IsNullOrWhiteSpace(updateParam.Name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<RoleDTO> repResult = await _rolesRepository.GetRoleByName(updateParam.Name);
            if (repResult.ErrorCode != (int)ErrorCodes.NotFound)
            {
                if (repResult.Data.Id != updateParam.Id)
                {
                    result.ErrorCode = (int)ErrorCodes.Conflict;
                    result.ErrorMessage = $"Role with name {updateParam.Name} exist";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
            }

            Result repUpdateResult = await _rolesRepository.UpdateRole(
                updateParam.Id, updateParam.Name, updateParam.Description);
            if (repUpdateResult.ErrorCode == (int)ErrorCodes.Success)
            {
                repResult = await _rolesRepository.GetRole(updateParam.Id);
                if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
                {
                    result.ErrorCode = (int)ErrorCodes.NotFound;
                    result.ErrorMessage = $"User with item {updateParam.Id} not exist";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
                result.Data = repResult.Data.MapToRole();
                return result;
            }
            return result;
        }
    }
}

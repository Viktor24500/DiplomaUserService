using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Extensions;
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

        public async Task<Result<Role>> CreateRole(string name)
        {
            Result<Role> result = new Result<Role>();
            if (string.IsNullOrWhiteSpace(name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<RoleDTO> repResult = await _rolesRepository.GetRoleByName(name);
            if (repResult.ErrorCode == (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"Role with name {name} exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            repResult = await _rolesRepository.CreateRole(name);
            result.Data = repResult.Data.MapToRole();
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

        public async Task<Result<Role>> UpdateRole(int id, string name)
        {
            Result<Role> result = new Result<Role>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<RoleDTO> repResult = await _rolesRepository.GetRoleByName(name);
            if (repResult.ErrorCode == (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"Role with name {name} exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            repResult = await _rolesRepository.UpdateRole(id, name);
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
    }
}

using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.UsersRolesPermissions;
using SystemUserService.BusinessLogic.Extensions;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.DataAccess.DTO.Permissions;
using SystemUserService.DataAccess.DTO.Roles;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.DTO.UsersRolesPermissions;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.BusinessLogic.Services
{
    public class UserRolePermissionsService : IUserRolePermissionsService
    {
        private IPermissionsRepository _permissionsRepository;
        private IUsersRepository _usersRepository;
        private IRolesRepository _rolesRepository;
        private IUserRolePermissionsRepository _usersRolesPermissionsRepository;
        private ILogger<UserRolePermissionsService> _logger;
        public UserRolePermissionsService(ILogger<UserRolePermissionsService> logger,
            IRolesRepository rolesRepository, IPermissionsRepository permissionsRepository, IUsersRepository usersRepository, IUserRolePermissionsRepository usersRolesPermissionsRepository)
        {
            _usersRepository = usersRepository;
            _logger = logger;
            _rolesRepository = rolesRepository;
            _permissionsRepository = permissionsRepository;
            _usersRolesPermissionsRepository = usersRolesPermissionsRepository;
        }

        public async Task<Result<List<UserRolePermissions>>> GetAllUserRolePermissions()
        {
            Result<List<UserRolePermissionDTO>> repResult = await _usersRolesPermissionsRepository.GetAllUserRolePermissions();
            Result<List<UserRolePermissions>> result = new Result<List<UserRolePermissions>>();
            result.Data = repResult.Data.MapToUserRolePermissionsCollection();
            return result;
        }

        public async Task<Result<List<UserRolePermissions>>> GetUserRolePermissionsByPermissionId(int id)
        {
            Result<List<UserRolePermissions>> result = new Result<List<UserRolePermissions>>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<PermissionDTO> repPermissionResult = await _permissionsRepository.GetPermission(id);
            if (repPermissionResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Permission with id {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<List<UserRolePermissionDTO>> repResult = await _usersRolesPermissionsRepository.GetUserRolePermissionsByPermissionId(id);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"UserRolePermission with permission id  {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToUserRolePermissionsCollection();
            return result;
        }

        public async Task<Result<List<UserRolePermissions>>> GetUserRolePermissionsByRoleId(int id)
        {
            Result<List<UserRolePermissions>> result = new Result<List<UserRolePermissions>>();
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

            Result<List<UserRolePermissionDTO>> repResult = await _usersRolesPermissionsRepository.GetUserRolePermissionsByRoleId(id);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"UserRolePermission with role id  {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToUserRolePermissionsCollection();
            return result;
        }

        public async Task<Result<List<UserRolePermissions>>> GetUserRolePermissionsByUserId(int id)
        {
            Result<List<UserRolePermissions>> result = new Result<List<UserRolePermissions>>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<UserDTO> repUserResult = await _usersRepository.GetUser(id);
            if (repUserResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"User with id {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<List<UserRolePermissionDTO>> repResult = await _usersRolesPermissionsRepository.GetUserRolePermissionsByUserId(id);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"UserRolePermission with user id  {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToUserRolePermissionsCollection();
            return result;
        }
    }
}

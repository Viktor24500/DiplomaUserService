using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.BusinessLogic.Extensions;
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

        public async Task<Result<List<UserRole>>> CreateUserRoles(int userId, List<int> rolesId)
        {
            //Validate role id
            Result<List<UserRole>> result = new Result<List<UserRole>>();
            if (IntExtension.IsNegative(userId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "role id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<UserDTO> repUserResult = await _usersRepository.GetUser(userId);
            if (repUserResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Role with id {userId} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //Validate permission id
            foreach (int roleId in rolesId)
            {
                if (IntExtension.IsNegative(roleId))
                {
                    result.ErrorCode = (int)ErrorCodes.BadRequest;
                    result.ErrorMessage = "permission id can't be negative";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
            }
            Result<RoleDTO> repRoleResult = new Result<RoleDTO>();
            foreach (int roleId in rolesId)
            {
                repRoleResult = await _rolesRepository.GetRole(roleId);
                if (repRoleResult.ErrorCode == (int)ErrorCodes.NotFound)
                {
                    result.ErrorCode = (int)ErrorCodes.Conflict;
                    result.ErrorMessage = $"Permission with id {roleId} not exist";
                    _logger.LogError(result.ErrorMessage);
                    return result;
                }
            }
            Result repUserRoleResult = await _userRolesRepository.CreateUserRoles(userId, rolesId);
            if (repUserRoleResult.ErrorCode == (int)ErrorCodes.Success)
            {
                Result<List<UserRoleDTO>> repReult = await _userRolesRepository.GetUserRoleByUserId(userId);
                result.Data = repReult.Data.MapToUserRoleCollection();
                return result;
            }
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
                result.ErrorMessage = $"Role permission with role id  {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToUserRoleCollection();
            return result;
        }

        public async Task<Result<List<UserRole>>> GetUserRoleByUserId(int id)
        {
            Result<List<UserRole>> result = new Result<List<UserRole>>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<List<UserRoleDTO>> repResult = await _userRolesRepository.GetUserRoleByUserId(id);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Role permission with role id  {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToUserRoleCollection();
            return result;
        }

        public async Task<Result<List<UserRole>>> UpdateUserRole(int userRoleId, int userId, int roleId)
        {
            Result<List<UserRole>> result = new Result<List<UserRole>>();
            if (IntExtension.IsNegative(userRoleId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "userRoleId can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //Validate role id
            if (IntExtension.IsNegative(userId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "role id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<UserDTO> repUserResult = await _usersRepository.GetUser(userId);
            if (repUserResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Role with id {userId} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //Validate permission id

            if (IntExtension.IsNegative(roleId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "permission id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<RoleDTO> repRoleResult = new Result<RoleDTO>();
            repRoleResult = await _rolesRepository.GetRole(roleId);
            if (repRoleResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"Permission with id {roleId} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result repUserRoleResult = await _userRolesRepository.UpdateUserRole(userRoleId, userId, roleId);
            if (repUserRoleResult.ErrorCode == (int)ErrorCodes.Success)
            {
                Result<List<UserRoleDTO>> repReult = await _userRolesRepository.GetUserRoleByUserId(userId);
                result.Data = repReult.Data.MapToUserRoleCollection();
                return result;
            }
            return result;
        }
    }
}

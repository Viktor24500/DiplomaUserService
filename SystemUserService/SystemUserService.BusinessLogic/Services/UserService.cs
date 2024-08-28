using Microsoft.Extensions.Logging;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Extensions;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Common.Validators;
using SystemUserService.DataAccess.DTO.Permissions;
using SystemUserService.DataAccess.DTO.Users;
using SystemUserService.DataAccess.Repositories.Intefaces;

namespace SystemUserService.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private IUsersRepository _usersRepository;
        private IPermissionsRepository _permissionsRepository;
        private ILogger<UserService> _logger;
        private PasswordChecks _passwordChecks;
        public UserService(IUsersRepository usersRepository, ILogger<UserService> logger, IPermissionsRepository permissionsRepository, PasswordChecks passwordChecks)
        {
            _usersRepository = usersRepository;
            _logger = logger;
            _permissionsRepository = permissionsRepository;
            _passwordChecks = passwordChecks;
        }
        public async Task<Result<User>> CreateUser(string userName, string userPassword, bool isActive, int permissionId)
        {
            Result<User> result = new Result<User>();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPassword))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (IntExtension.IsNegative(permissionId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result passResult = _passwordChecks.isPasswordValid(userPassword);
            if (passResult.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                result.ErrorCode = passResult.ErrorCode;
                result.ErrorMessage = passResult.ErrorMessage;
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //Search permission
            Result<PermissionDTO> permissionResult = await _permissionsRepository.GetPermission(permissionId);
            if (permissionResult.ErrorCode != (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Sector with {permissionId} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //Search user
            Result<UserDTO> repResult = await _usersRepository.GetUserByUserName(userName);
            if (repResult.ErrorCode == (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"User with name {userName} exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            //hash password
            userPassword = null;

            repResult = await _usersRepository.CreateUser(userName, userPassword, isActive, permissionId);
            result.Data = repResult.Data.MapToUser();
            return result;
        }

        public async Task<Result<List<User>>> GetAllUsers()
        {
            Result<List<UserDTO>> repResult = await _usersRepository.GetAllUsers();
            Result<List<User>> result = new Result<List<User>>();
            result.Data = repResult.Data.MapToUsersCollection();
            return result;
        }

        public async Task<Result<User>> GetUserById(int id)
        {
            Result<User> result = new Result<User>();
            if (IntExtension.IsNegative(id))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            Result<UserDTO> repResult = await _usersRepository.GetUserByUserId(id);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"User with {id} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToUser();
            return result;
        }

        public async Task<Result<User>> GetUserByName(string name)
        {
            Result<User> result = new Result<User>();
            if (string.IsNullOrWhiteSpace(name))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<UserDTO> repResult = await _usersRepository.GetUserByUserName(name);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"User with {name} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            result.Data = repResult.Data.MapToUser();
            return result;
        }

        public async Task<Result<User>> UpdateUser(int userId, string userName, string userPassword, bool isActive, int permissionId)
        {
            Result<User> result = new Result<User>();
            if (IntExtension.IsNegative(userId) || IntExtension.IsNegative(permissionId))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "id can't be negative";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "name can't be null or empty";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result passResult = _passwordChecks.isPasswordValid(userPassword);
            if (passResult.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                result.ErrorCode = passResult.ErrorCode;
                result.ErrorMessage = passResult.ErrorMessage;
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<PermissionDTO> permissionResult = await _permissionsRepository.GetPermission(permissionId);
            if (permissionResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"Permission not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            Result<UserDTO> repResult = await _usersRepository.GetUserByUserName(userName);
            if (repResult.ErrorCode == (int)ErrorCodes.Success)
            {
                result.ErrorCode = (int)ErrorCodes.Conflict;
                result.ErrorMessage = $"User with name {userName} exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }

            repResult = await _usersRepository.UpdateUser(userId, userName, userPassword, isActive, permissionId);
            if (repResult.ErrorCode == (int)ErrorCodes.NotFound)
            {
                result.ErrorCode = (int)ErrorCodes.NotFound;
                result.ErrorMessage = $"User with {userId} not exist";
                _logger.LogError(result.ErrorMessage);
                return result;
            }
            repResult = await _usersRepository.GetUserByUserId(userId);
            result.Data = repResult.Data.MapToUser();
            return result;
        }
    }
}

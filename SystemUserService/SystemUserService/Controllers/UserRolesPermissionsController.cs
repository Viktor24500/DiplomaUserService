using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.UsersRolesPermissions;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Utility;

namespace SystemUserService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserRolePermissionsController : ControllerBase
    {
        private IUserRolePermissionsService _userRolePermissionsService;
        public UserRolePermissionsController(IUserRolePermissionsService userRolePermissions)
        {
            _userRolePermissionsService = userRolePermissions;
        }

        [Route("/userRolePermissions")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserRolePermissions()
        {
            Result<List<UserRolePermissions>> result = await _userRolePermissionsService.GetAllUserRolePermissions();
            if (result.ErrorCode == (int)ErrorCodes.Success)
            {
                return Ok(result.Data);
            }
            Utilities.HandleUnexpectedErrorCode(result);
            return StatusCode(500);
        }

        [Route("/userRolePermissionsByRoleId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRolePermissionsByRoleId(int id)
        {
            Result<List<UserRolePermissions>> result = await _userRolePermissionsService.GetUserRolePermissionsByRoleId(id);
            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.NotFound:
                    return NotFound(result.ErrorMessage);
                case (int)ErrorCodes.BadRequest:
                    return BadRequest(result.ErrorMessage);
                case (int)ErrorCodes.Success:
                    return Ok(result.Data);
                default:
                    Utilities.HandleUnexpectedErrorCode(result);
                    return StatusCode(500);
            }
        }


        [Route("/userRolePermissionsByUserId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRolePermissionsByUserId(int id)
        {
            Result<List<UserRolePermissions>> result = await _userRolePermissionsService.GetUserRolePermissionsByUserId(id);
            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.NotFound:
                    return NotFound(result.ErrorMessage);
                case (int)ErrorCodes.BadRequest:
                    return BadRequest(result.ErrorMessage);
                case (int)ErrorCodes.Success:
                    return Ok(result.Data);
                default:
                    Utilities.HandleUnexpectedErrorCode(result);
                    return StatusCode(500);
            }
        }

        [Route("/userRolePermissionsByPermissionId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRolePermissionsByPermissionId(int id)
        {
            Result<List<UserRolePermissions>> result = await _userRolePermissionsService.GetUserRolePermissionsByPermissionId(id);
            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.NotFound:
                    return NotFound(result.ErrorMessage);
                case (int)ErrorCodes.BadRequest:
                    return BadRequest(result.ErrorMessage);
                case (int)ErrorCodes.Success:
                    return Ok(result.Data);
                default:
                    Utilities.HandleUnexpectedErrorCode(result);
                    return StatusCode(500);
            }
        }
    }
}

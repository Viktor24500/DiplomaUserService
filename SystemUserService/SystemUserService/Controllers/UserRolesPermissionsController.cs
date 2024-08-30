using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.UsersRolesPermissions;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;

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
            throw new Exception("Could not get all roles with permission");
        }

        [Route("/userRolePermissionsByRoleId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRolePermissionsByRoleId(int id)
        {
            Result<List<UserRolePermissions>> result = await _userRolePermissionsService.GetUserRolePermissionsByRoleId(id);
            if (result.ErrorCode == (int)ErrorCodes.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }
            if (result.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }
            else
            {
                return Ok(result.Data);
            }
            throw new Exception("Could not get role");
        }


        [Route("/userRolePermissionsByUserId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRolePermissionsByUserId(int id)
        {
            Result<List<UserRolePermissions>> result = await _userRolePermissionsService.GetUserRolePermissionsByUserId(id);
            if (result.ErrorCode == (int)ErrorCodes.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }
            if (result.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }
            else
            {
                return Ok(result.Data);
            }
            throw new Exception("Could not get role");
        }

        [Route("/userRolePermissionsByPermissionId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRolePermissionsByPermissionId(int id)
        {
            Result<List<UserRolePermissions>> result = await _userRolePermissionsService.GetUserRolePermissionsByPermissionId(id);
            if (result.ErrorCode == (int)ErrorCodes.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }
            if (result.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }
            else
            {
                return Ok(result.Data);
            }
            throw new Exception("Could not get role");
        }
    }
}

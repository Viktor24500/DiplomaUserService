using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;

namespace SystemUserService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private IUserRoleService _userRoleService;
        public UserRoleController(IUserRoleService userRole)
        {
            _userRoleService = userRole;
        }

        [Route("/userRoles")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserRoles()
        {
            Result<List<UserRole>> result = await _userRoleService.GetAllUsersRoles();
            if (result.ErrorCode == (int)ErrorCodes.Success)
            {
                return Ok(result.Data);
            }
            throw new Exception("Could not get all roles with permission");
        }

        [Route("/userRolesByRoleId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRolesByRoleId(int id)
        {
            Result<List<UserRole>> result = await _userRoleService.GetUserRoleByRoleId(id);
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


        [Route("/userRolesByUserId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRolesByUserId(int id)
        {
            Result<List<UserRole>> result = await _userRoleService.GetUserRoleByUserId(id);
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

        [Route("/userRoles")]
        [HttpPost]
        public async Task<IActionResult> CreateUserRoles(int roleId, List<int> permissionsId)
        {
            Result<List<UserRole>> result = await _userRoleService.CreateUserRoles(roleId, permissionsId);
            if (result.ErrorCode == (int)ErrorCodes.Conflict)
            {
                return Conflict(result.ErrorMessage);
            }
            if (result.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }
            if (result.ErrorCode == (int)ErrorCodes.Success)
            {
                return Created("/roles", result.Data);
            }
            throw new Exception("Could not update role permission");
        }

        [Route("/userRoles/{userRoleId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserRoles(int userRoleId, int roleId, int permissionId)
        {
            Result<List<UserRole>> result = await _userRoleService.UpdateUserRole(userRoleId, roleId, permissionId);
            if (result.ErrorCode == (int)ErrorCodes.Success)
            {
                return Ok(result.Data);
            }
            if (result.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }
            if (result.ErrorCode == (int)ErrorCodes.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }
            if (result.ErrorCode == (int)ErrorCodes.Conflict)
            {
                return Conflict(result.ErrorMessage);
            }
            throw new Exception("Could not update role permission");
        }
    }
}

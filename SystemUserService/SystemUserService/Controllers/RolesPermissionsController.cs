using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Utility;

namespace SystemUserService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class RolesPermissionsController : ControllerBase
    {
        private IRolePermissionsService _rolePermissionService;
        public RolesPermissionsController(IRolePermissionsService rolePermission)
        {
            _rolePermissionService = rolePermission;
        }

        [Route("/rolesPermissions")]
        [HttpGet]
        public async Task<IActionResult> GetAllRolePermissions()
        {
            Result<List<RolesPermission>> result = await _rolePermissionService.GetAllRolePermissions();
            if (result.ErrorCode == (int)ErrorCodes.Success)
            {
                return Ok(result.Data);
            }
            Utilities.HandleUnexpectedErrorCode(result);
            return StatusCode(500);
        }

        [Route("/rolesPermissions/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetRolePermissions(int id)
        {
            Result<List<RolesPermission>> result = await _rolePermissionService.GetRolePermissionsByRoleId(id);
            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.NotFound:
                    return NotFound(result.ErrorMessage);
                case (int)ErrorCodes.BadRequest:
                    return BadRequest(result.ErrorMessage);
                case (int)ErrorCodes.Conflict:
                    return Conflict(result.ErrorMessage);
                default:
                    Utilities.HandleUnexpectedErrorCode(result);
                    return StatusCode(500);
            }
        }

        [Route("/rolesPermissions")]
        [HttpPost]
        public async Task<IActionResult> CreateRolePermissions(int roleId, List<int> permissionsId)
        {
            Result<List<RolesPermission>> result = await _rolePermissionService.CreateRolePermissions(roleId, permissionsId);
            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.Conflict:
                    return Conflict(result.ErrorMessage);
                case (int)ErrorCodes.BadRequest:
                    return BadRequest(result.ErrorMessage);
                case (int)ErrorCodes.Success:
                    return Ok(result.Data);
                default:
                    Utilities.HandleUnexpectedErrorCode(result);
                    return StatusCode(500);
            }
        }

        [Route("/rolesPermissions/{rolePermissionId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateRolePermissions(int rolePermissionId, int roleId, int permissionId)
        {
            Result<List<RolesPermission>> result = await _rolePermissionService.UpdateRolePermissions(rolePermissionId, roleId, permissionId);
            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.Success:
                    return Ok(result.Data);
                case (int)ErrorCodes.BadRequest:
                    return BadRequest(result.ErrorMessage);
                case (int)ErrorCodes.NotFound:
                    return NotFound(result.ErrorMessage);
                case (int)ErrorCodes.Conflict:
                    return Conflict(result.ErrorMessage);
                default:
                    Utilities.HandleUnexpectedErrorCode(result);
                    return StatusCode(500);
            }
        }
    }
}

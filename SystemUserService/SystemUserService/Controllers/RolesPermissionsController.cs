using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;

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
            throw new Exception("Could not get all roles with permission");
        }

        [Route("/rolesPermissions/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetRolePermissions(int id)
        {
            Result<List<RolesPermission>> result = await _rolePermissionService.GetRolePermissionsByRoleId(id);
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

        [Route("/rolesPermissions")]
        [HttpPost]
        public async Task<IActionResult> CreateRolePermissions(int roleId, List<int> permissionsId)
        {
            Result<List<RolesPermission>> result = await _rolePermissionService.CreateRolePermissions(roleId, permissionsId);
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

        [Route("/rolesPermissions/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateRolePermissions(int rolePermissionId, int roleId, int permissionId)
        {
            Result<List<RolesPermission>> result = await _rolePermissionService.UpdateRolePermissions(rolePermissionId, roleId, permissionId);
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

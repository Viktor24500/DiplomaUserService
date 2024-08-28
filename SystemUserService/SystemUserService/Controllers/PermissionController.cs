using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;

namespace SystemUserService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [Route("/permissions")]
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            Result<List<Permission>> result = await _permissionService.GetAllPermissions();
            if (result.ErrorCode == (int)ErrorCodes.Success)
            {
                return Ok(result.Data);
            }
            throw new Exception("Could not get all permissions");
        }

        [Route("/permissions/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPermission(int id)
        {
            Result<Permission> result = await _permissionService.GetPermission(id);
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
            throw new Exception("Could not get permission");
        }

        [Route("/permissions")]
        [HttpPost]
        public async Task<IActionResult> CreatePermission(string name, string? description)
        {
            Result<Permission> result = await _permissionService.CreatePermission(name, description);
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
                return Created("/permissions", result.Data);
            }
            throw new Exception("Could not create permission");
        }

        [Route("/permissions/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdatePermission(int id, string name, string? description)
        {
            Result<Permission> result = await _permissionService.UpdatePermission(id, name, description);
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
            throw new Exception("Could not create permission");
        }
    }
}

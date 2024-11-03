using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.Permissions;
using SystemUserService.BusinessLogic.Parametrs.Permissions;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Request.Permissions;
using SystemUserService.Utility;

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
            Utilities.HandleUnexpectedErrorCode(result);
            return StatusCode(500);
        }

        [Route("/permissions/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPermission(int id)
        {
            Result<Permission> result = await _permissionService.GetPermission(id);
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

        [Route("/permissions")]
        [HttpPost]
        public async Task<IActionResult> CreatePermission(PermissionCreateRequest createRequest)
        {
            PermissionCreateParametrs createParam = new PermissionCreateParametrs(
                createRequest.Name, createRequest.Description);
            Result<Permission> result = await _permissionService.CreatePermission(createParam);
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

        [Route("/permissions/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdatePermission(PermissionUpdateRequest updateRequest)
        {
            PermissionUpdateParametrs updateParam = new PermissionUpdateParametrs(
                updateRequest.Id, updateRequest.Name, updateRequest.Description);
            Result<Permission> result = await _permissionService.UpdatePermission(updateParam);
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

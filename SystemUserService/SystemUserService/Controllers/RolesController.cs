using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Utility;

namespace SystemUserService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Route("/roles")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            Result<List<Role>> result = await _roleService.GetAllRoles();
            if (result.ErrorCode == (int)ErrorCodes.Success)
            {
                return Ok(result.Data);
            }
            Utilities.HandleUnexpectedErrorCode(result);
            return StatusCode(500);
        }

        [Route("/roles/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetRole(int id)
        {
            Result<Role> result = await _roleService.GetRole(id);
            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.Success:
                    return Ok(result.Data);
                case (int)ErrorCodes.NotFound:
                    return NotFound(result.ErrorMessage);
                case (int)ErrorCodes.BadRequest:
                    return BadRequest(result.ErrorMessage);
                default:
                    Utilities.HandleUnexpectedErrorCode(result);
                    return StatusCode(500);
            }

        }

        [Route("/roles")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(string name, string? description)
        {
            Result<Role> result = await _roleService.CreateRole(name, description);

            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.Success:
                    return Ok(result.Data);
                case (int)ErrorCodes.Conflict:
                    return Conflict(result.ErrorMessage);
                case (int)ErrorCodes.BadRequest:
                    return BadRequest(result.ErrorMessage);
                default:
                    Utilities.HandleUnexpectedErrorCode(result);
                    return StatusCode(500);
            }
        }

        [Route("/roles/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateRole(int id, string name, string? description)
        {
            Result<Role> result = await _roleService.UpdateRole(id, name, description);
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

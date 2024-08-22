using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.Roles;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.ErrorCodes;
using SystemUserService.Common.Results;

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
            throw new Exception("Could not get all roles");
        }

        [Route("/roles/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetRole(int id)
        {
            Result<Role> result = await _roleService.GetRole(id);
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

        [Route("/roles")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            Result<Role> result = await _roleService.CreateRole(name);
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
            throw new Exception("Could not create role");
        }

        [Route("/roles/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateRole(int id, string name)
        {
            Result<Role> result = await _roleService.UpdateRole(id, name);
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
            throw new Exception("Could not create role");
        }
    }
}

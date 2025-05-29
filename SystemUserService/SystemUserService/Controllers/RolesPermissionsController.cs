using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.RolesPermission;
using SystemUserService.BusinessLogic.Parametrs.RolePermission;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Request.RolePermission;
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
			Result<List<RolePermission>> result = await _rolePermissionService.GetAllRolePermissions();
			if (result.ErrorCode == (int)ErrorCodes.Success)
			{
				return Ok(result.Data);
			}
			Utilities.HandleUnexpectedErrorCode(result);
			return StatusCode(500, result.ErrorMessage);
		}

		[Route("/rolesPermissions/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetRolePermissions(int id)
		{
			Result<RolePermission> result = await _rolePermissionService.GetRolePermissionsByRoleId(id);
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
					return StatusCode(500, result.ErrorMessage);
			}
		}

		[Route("/rolesPermissions")]
		[HttpPost]
		public async Task<IActionResult> CreateRolePermissions([FromBody] RolePermissionCreateRequest rolePermissionCreateRequest)
		{
			RolePermissionCreateParameters rolePermissionCreateParameters = new RolePermissionCreateParameters(rolePermissionCreateRequest.RoleId,
				rolePermissionCreateRequest.PermissionsId);
			Result<RolePermission> result = await _rolePermissionService.CreateRolePermissions(rolePermissionCreateParameters);
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
					return StatusCode(500, result.ErrorMessage);
			}
		}

		[Route("/rolesPermissions/{rolePermissionId}")]
		[HttpPut]
		public async Task<IActionResult> UpdateRolePermissions([FromBody] RolePermissionUpdateRequest rolePermissionUpdateRequest)
		{
			RolePermissionUpdateParameters rolePermissionUpdateParameters = new RolePermissionUpdateParameters(
				rolePermissionUpdateRequest.RolePermissionId, rolePermissionUpdateRequest.RoleId, rolePermissionUpdateRequest.PermissionId);
			Result<RolePermission> result = await _rolePermissionService.UpdateRolePermissions(rolePermissionUpdateParameters);
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
					return StatusCode(500, result.ErrorMessage);
			}
		}
	}
}

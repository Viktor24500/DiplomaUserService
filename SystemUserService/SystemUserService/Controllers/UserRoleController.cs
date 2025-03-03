using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.UsersRoles;
using SystemUserService.BusinessLogic.Parametrs.UserRole;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Request.UserRole;
using SystemUserService.Utility;

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
			Utilities.HandleUnexpectedErrorCode(result);
			return StatusCode(500);
		}

		[Route("/searchUserRoles/{name}")]
		[HttpGet]
		public async Task<IActionResult> SearchUserRolesByUserName(string name)
		{
			Result<List<UserRole>> result = await _userRoleService.SearchUserRolesByUserName(name);
			if (result.ErrorCode == (int)ErrorCodes.Success)
			{
				return Ok(result.Data);
			}
			Utilities.HandleUnexpectedErrorCode(result);
			return StatusCode(500);
		}

		[Route("/userRolesByRoleId/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetUserRolesByRoleId(int id)
		{
			Result<List<UserRole>> result = await _userRoleService.GetUserRoleByRoleId(id);
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


		[Route("/userRolesByUserId/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetUserRolesByUserId(int id)
		{
			Result<UserRole> result = await _userRoleService.GetUserRoleByUserId(id);
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

		[Route("/userRoles")]
		[HttpPost]
		public async Task<IActionResult> CreateUserRoles([FromBody] UserRoleCreateRequest userRoleCreateRequst)
		{
			UserRoleCreateParameters userRoleCreateParam = new UserRoleCreateParameters(
				userRoleCreateRequst.UserId, userRoleCreateRequst.RoleId);
			Result<UserRole> result = await _userRoleService.CreateUserRole(userRoleCreateParam);
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

		[Route("/userRoles/{userRoleId}")]
		[HttpPut]
		public async Task<IActionResult> UpdateUserRoles([FromBody] UserRoleUpdateRequest userRoleUpdateRequest)
		{
			UserRoleUpdateParameters userRoleUpdateParam = new UserRoleUpdateParameters(
				userRoleUpdateRequest.UserRoleId, userRoleUpdateRequest.UserId, userRoleUpdateRequest.RoleId);
			Result<UserRole> result = await _userRoleService.UpdateUserRole(userRoleUpdateParam);
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

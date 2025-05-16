using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.Logins;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Parametrs.Login;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Request.Login;
using SystemUserService.Request.User;
using SystemUserService.Utility;

namespace SystemUserService.Controllers
{
	//[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private IUserService _userService;
		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[Route("/users")]
		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			Result<List<User>> result = await _userService.GetAllUsers();
			if (result.ErrorCode == (int)ErrorCodes.Success)
			{
				return Ok(result.Data);
			}
			Utilities.HandleUnexpectedErrorCode(result);
			return StatusCode(500);
		}

		[Route("/users/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetUser(int id)
		{
			Result<User> result = await _userService.GetUser(id);
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

		[Route("/usersIsActive/{isActive}")]
		[HttpGet]
		public async Task<IActionResult> GetUserByActiveStatus(bool isActive)
		{
			Result<List<User>> result = await _userService.GetUserByActiveStatus(isActive);

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

		[Route("/users")]
		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest userCreateRequest)
		{
			DateTime dateRegistered = DateTime.Now;
			//TimeZoneInfo timeZone = TimeZoneInfo.Local;
			//TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time"); //local
			TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Kyiv");
			DateTime dateRegisteredLocalDateTime = TimeZoneInfo.ConvertTime(dateRegistered, timeZone);

			DateTime? lastLogin = null;
			UserCreateParameters userCreateParam = new UserCreateParameters(userCreateRequest.Username, userCreateRequest.UserPassword,
				userCreateRequest.Email, userCreateRequest.FirstName,
				userCreateRequest.LastName, userCreateRequest.Comment, userCreateRequest.IsActive,
				dateRegisteredLocalDateTime, lastLogin, userCreateRequest.PhoneNumber);
			Result<User> result = await _userService.CreateUser(userCreateParam);

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

		[Route("/users/{id}")]
		[HttpPut]
		public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest userUpdateRequest)
		{
			UserUpdateParameters userUpdateParam = new UserUpdateParameters(userUpdateRequest.Id,
				userUpdateRequest.Email, userUpdateRequest.FirstName, userUpdateRequest.LastName, userUpdateRequest.Comment,
				userUpdateRequest.IsActive, userUpdateRequest.PhoneNumber);
			Result<User> result = await _userService.UpdateUser(userUpdateParam);

			switch (result.ErrorCode)
			{
				case (int)ErrorCodes.Success:
					return Ok(result.Data);
				case (int)ErrorCodes.Conflict:
					return Conflict(result.ErrorMessage);
				case (int)ErrorCodes.BadRequest:
					return BadRequest(result.ErrorMessage);
				case (int)ErrorCodes.NotFound:
					return NotFound(result.ErrorMessage);
				default:
					Utilities.HandleUnexpectedErrorCode(result);
					return StatusCode(500);
			}
		}
		[HttpPost]
		[Route("/users/login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest login)
		{
			LoginParametrs loginParam = new LoginParametrs(login.Name, login.Password);
			Result<Login> result = await _userService.LoginUser(loginParam);
			switch (result.ErrorCode)
			{
				case (int)ErrorCodes.Success:
					return Ok(result.Data);
				case (int)ErrorCodes.BadRequest:
					return BadRequest(result.ErrorMessage);
				case (int)ErrorCodes.Forbidden:
					return StatusCode(403, result.ErrorMessage);
				default:
					Utilities.HandleUnexpectedErrorCode(result);
					return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("/user/{token}")]
		public async Task<IActionResult> GetUserByToken(string token)
		{
			Result<Login> result = await _userService.GetUserByToken(token);
			switch (result.ErrorCode)
			{
				case (int)ErrorCodes.Success:
					return Ok(result.Data);
				case (int)ErrorCodes.BadRequest:
					return BadRequest(result.ErrorMessage);
				case (int)ErrorCodes.NotFound:
					return NotFound(result.ErrorMessage);
				default:
					Utilities.HandleUnexpectedErrorCode(result);
					return StatusCode(500);
			}
		}
	}
}

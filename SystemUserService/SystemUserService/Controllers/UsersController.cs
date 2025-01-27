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

		//TODO YP: погана назва і не відповідає кнвенції вище
		//всі записи по отриманню списку юзерів можна обєднати в один АПІ з параметрами фільтруванням
		//GET /users? status = active     # Get all active users
		//GET /users? status = inactive   # Get all inactive users
		//GET /users                   # Get all users regardless of status (optional)
		[Route("/usersIsActive/{isActive}")]
		[HttpGet]
		public async Task<IActionResult> GetUserByActiveStatus(bool isActive)
		{
			//TODO YP: погана назва і як я написав вище це все може робити один метод
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
			DateTime? lastLogin = null;
			UserCreateParameters userCreateParam = new UserCreateParameters(userCreateRequest.Username, userCreateRequest.UserPassword,
				userCreateRequest.Email, userCreateRequest.FirstName,
				userCreateRequest.LastName, userCreateRequest.FatherName,
				dateRegistered, lastLogin, userCreateRequest.IsActive);
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
				userUpdateRequest.Email, userUpdateRequest.FirstName, userUpdateRequest.LastName, userUpdateRequest.FatherName,
				userUpdateRequest.IsActive);
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

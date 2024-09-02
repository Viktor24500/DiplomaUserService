using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;

namespace SystemUserService.Controllers
{
    [Route("api/[controller]")]
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
            throw new Exception("Could not get all users");
        }

        [Route("/users/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            Result<User> result = await _userService.GetUser(id);
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
            throw new Exception("Could not get user");
        }

        [Route("/usersIsActive/{isActive}")]
        [HttpGet]
        public async Task<IActionResult> GetUserByIsActive(bool isActive)
        {
            Result<List<User>> result = await _userService.GetUserByIsActive(isActive);
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
            throw new Exception("Could not get user");
        }

        [Route("/users")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(string username, string userPassword, string email,
                       string firstName, string lastName, string? fatherName,
                       DateTime dateRegistered, DateTime? lastLogin, bool isActive)
        {
            Result<User> result = await _userService.CreateUser(username, userPassword, email, firstName, lastName, fatherName, dateRegistered,
                lastLogin, isActive);
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
                return Created("/users", result.Data);
            }
            throw new Exception("Could not create user");
        }

        [Route("/users/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, string email, string firstName, string lastName, string? fatherName, bool isActive)
        {
            Result<User> result = await _userService.UpdateUser(id, email, firstName, lastName, fatherName, isActive);
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
            throw new Exception("Could not update user");
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username, string userpassword)
        {
            Result<string> result = await _userService.LoginUser(username, userpassword);
            if (result.ErrorCode == (int)ErrorCodes.Success)
            {
                return Ok(result.Data);
            }
            if (result.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }
            throw new Exception("Could not login user");
        }
    }
}

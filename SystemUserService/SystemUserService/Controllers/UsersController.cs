using Microsoft.AspNetCore.Mvc;
using SystemUserService.BusinessLogic.Entities.Users;
using SystemUserService.BusinessLogic.Parametrs.Login;
using SystemUserService.BusinessLogic.Services.Interfaces;
using SystemUserService.Common.Enums;
using SystemUserService.Common.Results;
using SystemUserService.Request.Login;
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
        public async Task<IActionResult> CreateUser(string username, string userPassword, string email,
                       string firstName, string lastName, string? fatherName, bool isActive)
        {
            DateTime dateRegistered = DateTime.Now;
            DateTime? lastLogin = null;
            Result<User> result = await _userService.CreateUser(username, userPassword, email, firstName, lastName, fatherName, dateRegistered,
                lastLogin, isActive);

            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.Success:
                    return Ok(result.Data);
                case (int)ErrorCodes.Conflict:
                    return NotFound(result.ErrorMessage);
                case (int)ErrorCodes.BadRequest:
                    return BadRequest(result.ErrorMessage);
                default:
                    Utilities.HandleUnexpectedErrorCode(result);
                    return StatusCode(500);
            }
        }

        [Route("/users/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, string email, string firstName, string lastName, string? fatherName, bool isActive)
        {
            Result<User> result = await _userService.UpdateUser(id, email, firstName, lastName, fatherName, isActive);

            switch (result.ErrorCode)
            {
                case (int)ErrorCodes.Success:
                    return Ok(result.Data);
                case (int)ErrorCodes.Conflict:
                    return NotFound(result.ErrorMessage);
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
            Result<string> result = await _userService.LoginUser(loginParam);
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
    }
}

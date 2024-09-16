using Microsoft.AspNetCore.Mvc;
using System;
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
            //TODO YP: якщо тут очікується лише саксес а може прийти щось інше, то те що прийшло треба помістити в ексепшен
            //щоб залогувати наприклад throw new Exception($"Could not get all users. Unexpected error code {result.ErrorCode}");
            //а оскільки це зустрічається в багатьох місцях, то краще написати десь в утилітах
            //метод типу HandleUnexpectedErrorCode
            throw new Exception("Could not get all users");
        }

        [Route("/users/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            Result<User> result = await _userService.GetUser(id);
            //TODO YP: тут краще switch
            if (result.ErrorCode == (int)ErrorCodes.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }
            if (result.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }
            //TODO YP: тут повинен аналізуватись ерор код інакше якщо це не нотфаунд і не бедреквест і не саксесс то 
            //повернеться саксeсс, що неправильно
            else
            {
                return Ok(result.Data);
            }
            //TODO YP: те саме що і вище
            throw new Exception("Could not get user");
        }

        //TODO YP: погана назва і не відповідає кнвенції вище
        //всі записи по отриманню списку юзерів можна обєднати в один АПІ з параметрами фільтруванням
        //GET /users? status = active     # Get all active users
        //GET /users? status = inactive   # Get all inactive users
        //GET /users                   # Get all users regardless of status (optional)
        [Route("/usersIsActive/{isActive}")]
        [HttpGet]
        //TODO YP: погана назва 
        //можна GetUserByActiveStatus
        public async Task<IActionResult> GetUserByIsActive(bool isActive)
        {
            //TODO YP: погана назва і як я написав вище це все може робити один метод
            Result<List<User>> result = await _userService.GetUserByIsActive(isActive);
            //TODO YP: тут краще switch
            if (result.ErrorCode == (int)ErrorCodes.NotFound)
            {
                return NotFound(result.ErrorMessage);
            }
            if (result.ErrorCode == (int)ErrorCodes.BadRequest)
            {
                return BadRequest(result.ErrorMessage);
            }
            //TODO YP: тут повинен аналізуватись ерор код інакше якщо це не нотфаунд і не бедреквест і не саксесс то 
            //повернеться саксасс, що неправильно
            else
            {
                return Ok(result.Data);
            }
            //TODO YP: те саме що і вище
            throw new Exception("Could not get user");
        }
      
        [Route("/users")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(string username, string userPassword, string email,
                       string firstName, string lastName, string? fatherName,
                       //TODO YP: DateTime dateRegistered, DateTime? lastLogin не повинны передаватись в апы, вони повинны самы фыксуватись системою
                       DateTime dateRegistered, DateTime? lastLogin, bool isActive)
        {
            Result<User> result = await _userService.CreateUser(username, userPassword, email, firstName, lastName, fatherName, dateRegistered,
                lastLogin, isActive);
            //TODO YP: тут краще switch
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
            //TODO YP: те саме що і вище
            throw new Exception("Could not create user");
        }

        [Route("/users/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, string email, string firstName, string lastName, string? fatherName, bool isActive)
        {
            Result<User> result = await _userService.UpdateUser(id, email, firstName, lastName, fatherName, isActive);
            //TODO YP: тут краще switch
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
            //TODO YP: те саме що і вище
            throw new Exception("Could not update user");
        }
        [HttpPost]
        //TODO YP: погана назва і не відповідає всій конвенції що вище
        //краще "/users/login"
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
            //TODO YP: те саме що і вище
            throw new Exception("Could not login user");
        }
    }
}

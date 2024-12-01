using Microsoft.AspNetCore.Mvc;
using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.DTOs.User;

namespace MailManagement_vav0256.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            var user = _userService.Login(loginDto.Email, loginDto.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var userData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user.Email + ":" + user.Role));
            return Ok(userData);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!IsAuthorized())
            {
                return Unauthorized();
            }

            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            if (!IsAuthorized())
            {
                return Unauthorized();
            }

            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserCreateDto userDto)
        {
            if (!IsAuthorized("Administrator"))
            {
                return Forbid();
            }

            var user = _userService.CreateUser(userDto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] UserUpdateDto userDto)
        {
            if (!IsAuthorized("Administrator"))
            {
                return Forbid();
            }

            var updated = _userService.UpdateUser(id, userDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!IsAuthorized("Administrator"))
            {
                return Forbid();
            }

            var deleted = _userService.DeleteUser(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private bool IsAuthorized(string requiredRole = null)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return false;
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            var authData = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authHeader));
            var role = authData.Split(':')[1];

            if (requiredRole != null)
                return role == requiredRole || role == "Administrator";

            return true;
        }
    }
}
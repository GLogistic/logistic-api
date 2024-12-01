using Contracts.Services;
using Entities.Exceptions;
using Entities.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Base;

namespace MVCApp.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthController : GuestController
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        [HttpPost("login", Name = "login")]
        public async Task<IActionResult> Login([FromBody] UserAuthorizationDto dto)
        {
            try
            {
                var token = await _authService.AuthorizeAsync(dto);

                if (token == null)
                    return Unauthorized("Credentials is invalid");

                Response.Cookies.Append("Bearer", token.Token.Token.ToString(), new CookieOptions { HttpOnly = true, Expires = token.Token.Expire, SameSite = SameSiteMode.Strict });

                var user = await _userService.GetByIdAsync<UserDto>(token.UserId);

                var role = await _userService.GetUserRoleById(token.UserId);

                user.Role = role;

                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return Unauthorized();
            }
        }
        [HttpPost("register", Name = "register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            var isRegister = await _authService.RegisterAsync(dto, ["User"]);

            if (!isRegister)
                return BadRequest();

            return Ok();
        }
    }
}

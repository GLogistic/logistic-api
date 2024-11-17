using Contracts.Services;
using Entities.Exceptions;
using Entities.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Base;
using System.Net;

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
        [HttpGet("login", Name = "login-view")]
        public IActionResult LoginView()
        {
            return View();
        }
        [HttpPost("login", Name = "login")]
        public async Task<IActionResult> Login([FromBody] UserAuthorizationDto dto)
        {
            try
            {
                var token = await _authService.AuthorizeAsync(dto);

                if (token == null)
                    return Unauthorized();

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
        [HttpGet("register", Name = "register-view")]
        public IActionResult RegisterView()
        {
            return View();
        }
        [HttpPost("register", Name = "register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            var isRegister = await _authService.RegisterAsync(dto, ["User"]);

            if (!isRegister)
                return RedirectToAction("RegisterView");

            return RedirectToAction("LoginView");
        }
    }
}

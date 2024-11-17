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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet("login", Name = "login-view")]
        public IActionResult LoginView()
        {
            return View();
        }
        [HttpPost("login", Name = "login")]
        public async Task<IActionResult> Login([FromForm] UserAuthorizationDto dto)
        {
            try
            {
                var token = await _authService.AuthorizeAsync(dto);

                if (token == null)
                    return RedirectToAction("LoginView");

                Response.Cookies.Append("Bearer", token.Token.ToString(), new CookieOptions { HttpOnly = true, Expires = token.Expire, SameSite = SameSiteMode.Strict });

                return RedirectToAction("Index", "Home");
            }
            catch (NotFoundException ex)
            {
                return RedirectToAction("LoginView");
            }
        }
        [HttpGet("register", Name = "register-view")]
        public IActionResult RegisterView()
        {
            return View();
        }
        [HttpPost("register", Name = "register")]
        public async Task<IActionResult> Register([FromForm] UserRegistrationDto dto)
        {
            var isRegister = await _authService.RegisterAsync(dto, ["User"]);

            if (!isRegister)
                return RedirectToAction("RegisterView");

            return RedirectToAction("LoginView");
        }
    }
}

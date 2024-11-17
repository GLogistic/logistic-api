using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Attributes;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles]
    [Route("logout")]
    [ApiController]
    public class LogoutController : Controller
    {
        [HttpPost("", Name = "logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("Bearer", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                Path = "/"
            });

            return RedirectToAction("LoginView", "Auth");
        }
    }
}

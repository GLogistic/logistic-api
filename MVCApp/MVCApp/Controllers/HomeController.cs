using Entities;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Attributes;
using MVCApp.Controllers.Base;

namespace MVCApp.Controllers
{
    [AuthorizeByRoles("Admin", "User")]
    [Route("")]
    [ApiController]
    public class HomeController : BaseController
    {

        public HomeController() { }

        public IActionResult Index()
        {
            return View();
        }
    }
}

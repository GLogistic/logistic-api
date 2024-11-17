using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MVCApp.Controllers.Helpers;

namespace MVCApp.Controllers.Base
{
    public class BaseController : Controller
    {
        public BaseController() { }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Layout"] = HttpContext.IsAdmin() ? Constants.AdminLayout : Constants.UserLayout;
        }
    }
}

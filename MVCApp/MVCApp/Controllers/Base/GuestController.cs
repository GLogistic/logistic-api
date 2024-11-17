using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MVCApp.Controllers.Helpers;

namespace MVCApp.Controllers.Base
{
    public class GuestController : Controller
    {
        public GuestController() { }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Layout"] = Constants.GuestLayout;
        }
    }
}

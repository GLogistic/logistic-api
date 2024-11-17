using Microsoft.AspNetCore.Mvc;
using MVCApp.Controllers.Filters;

namespace MVCApp.Controllers.Attributes
{
    public class AuthorizeByRolesAttribute : TypeFilterAttribute
    {
        public AuthorizeByRolesAttribute(params string[] roles)
        : base(typeof(AuthorizeByRolesFilter))
        {
            Arguments = new object[] { roles };
        }
    }
}

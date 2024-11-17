using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVCApp.Controllers.Filters
{
    public class AuthorizeByRolesFilter : IAuthorizationFilter
    {
        private readonly IAuthService _authService;
        private readonly string[] _roles;
        private const string TokenCookieName = "Bearer";

        public AuthorizeByRolesFilter(IAuthService authService, params string[] roles)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var httpContext = context.HttpContext;
                var token = httpContext.Request.Cookies[TokenCookieName];

                if (string.IsNullOrEmpty(token))
                {
                    context.HttpContext.Response.Redirect("/login");
                    //context.Result = new UnauthorizedResult();
                    return;
                }

                var jwtHandler = new JwtSecurityTokenHandler();
                if (!jwtHandler.CanReadToken(token))
                {
                    context.HttpContext.Response.Redirect("/login");
                    //context.Result = new UnauthorizedResult();
                    return;
                }

                JwtSecurityToken jwtToken = null;

                if (!_authService.IsValidToken(token, out jwtToken))
                {
                    context.HttpContext.Response.Redirect("/login");
                    //context.Result = new UnauthorizedResult();
                    return;
                }

                if (jwtToken == null)
                {
                    context.HttpContext.Response.Redirect("/login");
                    //context.Result = new UnauthorizedResult();
                    return;
                }

                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userId != null && !_authService.IsUserExistById(new Guid(userId.Value)))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                if (!_authService.IsValidRoles(jwtToken, _roles))
                {
                    context.HttpContext.Response.Redirect("/");
                    //context.Result = new ForbidResult();
                    return;
                }

                context.HttpContext.Items["isAdmin"] = _authService.IsAdmin(jwtToken);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authorization Error: {ex.Message}");

                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

namespace MVCApp.Controllers.Helpers
{
    public static class HttpContextExtensions
    {
        public static bool IsAdmin(this HttpContext context)
        {
            if (context.Items.TryGetValue("isAdmin", out var isAdmin))
                return (bool)isAdmin;

            return false;
        }
    }
}

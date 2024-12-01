namespace Entities.ServiceHelpers
{
    public class AuthorizeResult
    {
        public Jwt Token { get; set; }
        public Guid UserId { get; set; }
    }
}

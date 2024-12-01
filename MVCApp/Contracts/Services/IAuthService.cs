using Entities.Models.DTOs.User;
using Entities.ServiceHelpers;
using System.IdentityModel.Tokens.Jwt;
namespace Contracts.Services
{
    public interface IAuthService
    {
        bool IsUserExistById(Guid id);
        bool IsValidToken(string token, out JwtSecurityToken? jwtSecurityToken);
        bool IsValidRoles(JwtSecurityToken jwtToken, IEnumerable<string> roles);
        bool IsAdmin(JwtSecurityToken jwtToken);
        Task<AuthorizeResult?> AuthorizeAsync(UserAuthorizationDto userAuthorizationDto);
        Task<bool> RegisterAsync(UserRegistrationDto userRegistrationDto, IEnumerable<string> roles);
    }
}

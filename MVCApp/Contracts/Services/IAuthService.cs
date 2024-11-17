using Entities;
using Entities.Models.DTOs.User;
using System.IdentityModel.Tokens.Jwt;
namespace Contracts.Services
{
    public interface IAuthService
    {
        bool IsUserExistById(Guid id);
        bool IsValidToken(string token, out JwtSecurityToken? jwtSecurityToken);
        bool IsValidRoles(JwtSecurityToken jwtToken, IEnumerable<string> roles);
        bool IsAdmin(JwtSecurityToken jwtToken);
        Task<Jwt?> AuthorizeAsync(UserAuthorizationDto userAuthorizationDto);
        Task<bool> RegisterAsync(UserRegistrationDto userRegistrationDto, IEnumerable<string> roles);
    }
}

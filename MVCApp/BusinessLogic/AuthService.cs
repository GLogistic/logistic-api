using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Entities.Models.DTOs.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogic
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapperService _mapperService;

        private readonly string? _issuer;
        private readonly string? _audience;
        private readonly int _expireInDay;
        private readonly string? _securityKey;

        private readonly TokenValidationParameters _validationParameters;

        public AuthService(IUserRepository repository, IMapperService mapperService, IConfiguration configuration)
        {
            _userRepository = repository;
            _mapperService = mapperService;

            var jwtSetting = configuration.GetSection("Jwt");

            _issuer = jwtSetting.GetSection("Issuer").Value;
            _audience = jwtSetting.GetSection("Audience").Value;
            _expireInDay = Int32.Parse(jwtSetting.GetSection("ExpiresInDay").Value);
            _securityKey = jwtSetting.GetSection("SecurityKey").Value;

            _validationParameters = new TokenValidationParameters
            {
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey)),
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RoleClaimType = ClaimTypes.Role,
            };
        }

        public async Task<Jwt?> AuthorizeAsync(UserAuthorizationDto userAuthorizationDto)
        {
            var userByEmail = await _userRepository.FindByEmailAsync(userAuthorizationDto.Email);

            if (userByEmail == null)
                return null;

            if (!await _userRepository.CheckPasswordAsync(userByEmail, userAuthorizationDto.Password))
                return null;

            var id = userByEmail.Id;
            var roles = await _userRepository.GetUserRolesAsync(userByEmail);
            return CreateJwtToken(id, roles);
        }
        public async Task<bool> RegisterAsync(UserRegistrationDto userRegistrationDto, IEnumerable<string> roles)
        {

            var user = _mapperService.Map<UserRegistrationDto, User>(userRegistrationDto);

            if (user == null)
                return false;

            var registerUser = await _userRepository.CreateAsync(user, userRegistrationDto.Password, roles);

            if (registerUser == null)
                return false;

            return true;
        }

        private Jwt? CreateJwtToken(Guid id, IEnumerable<string> roles)
        {
            var claims = GetClaims(id, roles.ToArray());
            var signingCredentials = GetSigningCredentials();

            var expire = DateTime.Now.AddDays(_expireInDay);

            var tokenOptions = GetTokenOptions(claims, signingCredentials, expire);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new Jwt { Token = token, Expire = expire };
        }
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_securityKey);

            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private List<Claim> GetClaims(Guid id, string[] roles)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, id.ToString()) };

            for (var i = 0; i < roles.Length; i++)
                claims.Add(new Claim(ClaimTypes.Role, roles[i]));

            foreach (var claim in claims)
                Console.Write(claim);

            return claims;
        }
        private JwtSecurityToken GetTokenOptions(List<Claim> myClaims, SigningCredentials mySigningCredentials, DateTime expire)
        {
            return new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: myClaims,
                expires: expire,
                signingCredentials: mySigningCredentials
                );
        }

        public bool IsUserExistById(Guid id) => _userRepository.FindByCondition(u => u.Id == id).Any();

        public bool IsValidToken(string token, out JwtSecurityToken? jwtSecurityToken)
        {
            var handler = new JwtSecurityTokenHandler();
            jwtSecurityToken = null;
            try
            {
                SecurityToken resultToken = null;
                handler.ValidateToken(token, _validationParameters, out resultToken);
                jwtSecurityToken = (JwtSecurityToken)resultToken;
                return true;
            }
            catch (SecurityTokenValidationException ex)
            {
                return false;
            }
        }

        public bool IsValidRoles(JwtSecurityToken jwtToken, IEnumerable<string> roles)
        {
            var rolesFromJwt = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value.ToLower());

            return roles.Any(role => rolesFromJwt.Contains(role.ToLower()));
        }

        public bool IsAdmin(JwtSecurityToken jwtToken) =>
            jwtToken.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.ToLower().Contains("admin"));
    }
}

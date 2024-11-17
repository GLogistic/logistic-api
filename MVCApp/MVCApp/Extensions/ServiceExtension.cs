using AutoMapper;
using BusinessLogic;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using DbAccess.Context;
using DbAccess.Repositories;
using Entities;
using MapperHelper.Profiles;
using MapperHelper.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace MVCApp.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureContext(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<LogisticContext>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("LogisticConnection"),
                db => db.MigrationsAssembly("MVCApp")));
        }
        public static void ConfigureScopedDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICargoRepository, CargoRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<ISettlementRepository, SettlementRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ICargoService, CargoService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<ISettlementService, SettlementService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
        }
        public static void ConfigureMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(opt => opt.AddProfiles(new List<Profile>()
            {
                new CargoProfile(),
                new RouteProfile(),
                new SettlementProfile(),
                new UserProfile(),
            }));

            services.AddScoped<IMapper>(x => new Mapper(config));

            services.AddScoped<IMapperService, MapperService>();
        }
        public static void ConfigureCacheProfiles(this IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("EntityCache",
                    new CacheProfile()
                    {
                        Duration = 268,
                        Location = ResponseCacheLocation.Client,
                        NoStore = false,
                    });
            });
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<LogisticContext>();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.User.RequireUniqueEmail = true;

                opt.Password.RequiredLength = 10;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = true;

                opt.SignIn.RequireConfirmedPhoneNumber = false;
                opt.SignIn.RequireConfirmedEmail = false;
            });
        }
        public static void JwtConfigure(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var jwtSetting = builder.Configuration.GetSection("Jwt");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSetting.GetSection("Issuer").Value,
                    ValidAudience = jwtSetting.GetSection("Audience").Value,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.GetSection("SecurityKey").Value)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = ClaimTypes.Role,
                };
            });
        }
    }
}

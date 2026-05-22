using Application.Common.Interfaces;
using Domain.Authentication;
using Domain.Caching;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDistributedMemoryCache();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.Configure<JWTSettings>(config.GetSection("JwtSettings"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["JwtSettings:Issuer"],
                        ValidAudience = config["JwtSettings:Audience"],
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["JwtSettings:Secret"])),
                        ClockSkew = TimeSpan.Zero
                    };
                }
            );
            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}

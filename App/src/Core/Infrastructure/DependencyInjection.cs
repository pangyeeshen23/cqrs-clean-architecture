using Domain.Authentication;
using Domain.Caching;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.Context;
using Infrastructure.Repositories;
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
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher>();
            services.Configure<JWTSettings>(config.GetSection("JwtSettings"));
            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}

using System.Security.Claims;
using Application;
using Dapper;
using Domain.Caching;
using Infrastructure;
using Infrastructure.Caching.Redis;
using Infrastructure.Context;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Test.Core.Seeder;
using Testcontainers.MsSql;
using Testcontainers.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Test.Core
{
    public class TestHost
    {

        public async Task<IHost> Init(string msqlConnectionStr, string redisConnectionStr)
        {
           

            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices(async (context, services) =>
                {
                    IConfiguration config = context.Configuration;
                    services.AddSingleton(config);
                    services.AddApplication();
                    services.AddInfrastructure(config, true);
                    services.AddDbContext<MyDbContext>(options =>
                        options.UseSqlServer(msqlConnectionStr));
                    services.AddScoped<ICacheService, RedisCacheService>();
                    services.AddSingleton<IConnectionMultiplexer>(_ =>
                    {
                        var options = ConfigurationOptions.Parse(redisConnectionStr);
                        options.AbortOnConnectFail = false;
                        options.ConnectRetry = 3;
                        return ConnectionMultiplexer.Connect(options);
                    });
                    services.AddSingleton<DapperContext>();
                    services.AddSingleton<IHttpContextAccessor>(_ =>
                    {
                        var accessor = new HttpContextAccessor();

                        accessor.HttpContext = new DefaultHttpContext
                        {
                            User = new ClaimsPrincipal(
                                new ClaimsIdentity(
                                [
                                    new Claim(ClaimTypes.NameIdentifier, UserSeeder.Id.ToString()),
                                    new Claim(ClaimTypes.Name, UserSeeder.FullName)
                                ], "TestAuth"))
                        };

                        return accessor;
                    });
                })
                .Build();
        }
    }
}

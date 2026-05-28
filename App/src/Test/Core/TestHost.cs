using System.Security.Claims;
using Application;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Test.Core.Seeder;

namespace Test.Core
{
    public class TestHost
    {
        public IHost Thost { get; }
        public TestHost()
        {
            this.Thost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    IConfiguration config = context.Configuration;
                    services.AddSingleton(config);
                    services.AddApplication();
                    services.AddInfrastructure(config, true);
                    services.AddDbContext<MyDbContext>(options =>
                        options.UseInMemoryDatabase("MyDB-" + Guid.NewGuid().ToString()));
                    var httpContext = new DefaultHttpContext();
                    httpContext.User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            [
                                new Claim(ClaimTypes.NameIdentifier, UserSeeder.Id.ToString()),
                                new Claim(ClaimTypes.Name, UserSeeder.FullName.ToString()),
                            ], "TestAuth")
                        );
                    services.AddSingleton<IHttpContextAccessor>(
                        new HttpContextAccessor
                        {
                            HttpContext = httpContext
                        }
                    );
                })
                .Build();
        }
    }
}

using Application;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                        options.UseInMemoryDatabase("MyDB"));
                })
                .Build();
        }
    }
}

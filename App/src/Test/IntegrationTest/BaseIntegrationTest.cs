using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application;
using Domain.Authentication;
using Domain.Caching;
using Domain.Repositories;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Test.Core;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace Test.IntegrationTest
{
    [TestClass]
    public class BaseIntegrationTest
    {
        protected IHost? _host;
        protected IServiceScope? _scope;
        private static string msqlConnectionStr = string.Empty;
        private static string redisConnectionStr = string.Empty;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var msqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-latest").Build();
            msqlContainer.StartAsync().GetAwaiter().GetResult();
            msqlConnectionStr = msqlContainer.GetConnectionString();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(msqlConnectionStr);
            builder.InitialCatalog = "TestDb";
            msqlConnectionStr = builder.ConnectionString;

            var redisContainer = new RedisBuilder("redis:7.4-alpine").Build();
            redisContainer.StartAsync().GetAwaiter().GetResult();
            redisConnectionStr = redisContainer.GetConnectionString();

        }

        [TestInitialize]
        public virtual async Task TestSetup()
        {
            TestHost testHost = new TestHost();
            string dbName = $"TestDb_{Guid.NewGuid()}";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(msqlConnectionStr);
            builder.InitialCatalog = dbName;
            string testDbConnectionStr = builder.ConnectionString;
            _host = await testHost.Init(testDbConnectionStr, redisConnectionStr);
            _scope = _host!.Services.CreateScope();
            var db = _scope.ServiceProvider.GetRequiredService<MyDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}

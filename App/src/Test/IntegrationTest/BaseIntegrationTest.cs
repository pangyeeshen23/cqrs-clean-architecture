using System;
using System.Collections.Generic;
using System.Text;
using Application;
using Domain.Authentication;
using Domain.Caching;
using Domain.Repositories;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Test.Core;

namespace Test.IntegrationTest
{
    public class BaseIntegrationTest
    {
        protected TestHost? _host;

        [TestInitialize]
        public virtual void Setup()
        {
            _host = new TestHost();
            using var scope = _host.Thost.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}

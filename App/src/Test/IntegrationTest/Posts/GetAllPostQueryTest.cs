using System;
using System.Collections.Generic;
using System.Text;
using Application.Users.Queries.GetUserProfile;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Test.Core.Seeder;

namespace Test.IntegrationTest.Posts
{
    [TestClass]
    public class GetUserProfileTest : BaseIntegrationTest
    {
        private IMediator? _mediator;

        [TestInitialize]
        public override async Task TestSetup()
        {
            await base.TestSetup();
            _mediator = _scope!.ServiceProvider.GetRequiredService<IMediator>();
        }

        [TestMethod]
        public async Task GetUser_Should_Success()
        {
            UserSeeder userSeeder = new UserSeeder(
                _scope!.ServiceProvider.GetRequiredService<IUserRepository>(),
                _scope!.ServiceProvider.GetRequiredService<IPasswordHasher<User>>()
            );
            User user = await userSeeder.SeedUser();
            GetUserProfileQuery query = new GetUserProfileQuery(user.Id);
            GetUserProfileResponse resp = await _mediator!.Send(query);
            Assert.IsNotEmpty(resp.Username);
            Assert.IsNotEmpty(resp.Email);
            Assert.IsNotEmpty(resp.FullName);
            Assert.IsGreaterThan(0, resp.Age);
            Assert.IsNotEmpty(resp.PhoneNumber);
        }
    }
}

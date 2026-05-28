using System;
using System.Collections.Generic;
using System.Text;
using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Queries.GetUserProfile;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Test.Core.Seeder;

namespace Test.IntegrationTest.Users
{
    [TestClass]
    public class GetUserProfileTest : BaseIntegrationTest
    {
        private IMediator? _mediator;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            _mediator = _host?.Thost.Services.GetRequiredService<IMediator>();
        }

        [TestMethod]
        public async Task GetUser_Should_Success()
        {
            UserSeeder userSeeder = new UserSeeder(
                _host!.Thost.Services.GetRequiredService<IUserRepository>(),
                _host.Thost.Services.GetRequiredService<IPasswordHasher<User>>()
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

        [TestMethod]
        public async Task GetUser_Should_Failed_UserNotFoundException()
        {
            GetUserProfileQuery query = new GetUserProfileQuery(Guid.NewGuid());
            Func<Task> act = () => _mediator!.Send(query);
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}

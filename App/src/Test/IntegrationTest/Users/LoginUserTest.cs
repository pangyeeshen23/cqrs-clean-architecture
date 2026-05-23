using Application.Tags.Commnads.CreateTag;
using Application.Users.Commands.LoginUser;
using Domain.Authentication;
using Domain.Caching;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Users;
using Domain.Repositories;
using Domain.Repositories.Model.Users;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Test.Core.Seeder;
using Test.IntegrationTest;

namespace Test.UnitTest.Users
{
    [TestClass]
    public sealed class LoginUserTest : BaseIntegrationTest
    {
        private IMediator? _mediator;

        [TestInitialize]
        public void Setup()
        {
            base.Setup();
            _mediator = _host?.Thost.Services.GetRequiredService<IMediator>();
        }

        [TestMethod]
        public async Task UserLogin_Should_Failed_UserNotFoundException()
        {
            LoginUserCommand command = new LoginUserCommand("ethanpang", "!123qwe123");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [TestMethod]
        public async Task UserLogin_Should_Success()
        {
            UserSeeder userSeeder = new UserSeeder(
                _host!.Thost.Services.GetRequiredService<IUserRepository>(), 
                _host.Thost.Services.GetRequiredService<IPasswordHasher<User>>()
            );
            await userSeeder.SeedUser();
            LoginUserCommand command = new LoginUserCommand("ethanpang", "!root123Qwe123");
            var result = await _mediator!.Send(command);
            Assert.IsNotEmpty(result.Token);
        }
    }
}

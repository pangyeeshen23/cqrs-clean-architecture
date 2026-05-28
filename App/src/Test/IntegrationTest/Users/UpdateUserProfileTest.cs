using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Queries.GetUserProfile;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Test.Core.Seeder;

namespace Test.IntegrationTest.Users
{
    [TestClass]
    public class UpdateUserProfileTest : BaseIntegrationTest
    {
        private IMediator? _mediator;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            _mediator = _host?.Thost.Services.GetRequiredService<IMediator>();
        }

        [TestMethod]
        public async Task UpdateUser_Should_Success()
        {
            UserSeeder userSeeder = new UserSeeder(
                _host!.Thost.Services.GetRequiredService<IUserRepository>(),
                _host.Thost.Services.GetRequiredService<IPasswordHasher<User>>()
            );
            User user = await userSeeder.SeedUser();
            UpdateUserProfileCommand command = new UpdateUserProfileCommand("Ethan Pang", 50, "60128272851");
            UpdateUserProfileResponse resp = await _mediator!.Send(command);
            GetUserProfileQuery query = new GetUserProfileQuery(user.Id);
            GetUserProfileResponse profileResp = await _mediator!.Send(query);
            Assert.AreEqual<string>("Ethan Pang", profileResp.FullName);
            Assert.AreEqual<int>(50, profileResp.Age);
            Assert.AreEqual<string>("60128272851", profileResp.PhoneNumber);
        }


        [TestMethod]
        public async Task UpdateUser_Should_Fail_UserNotFoundException()
        {
            UserSeeder userSeeder = new UserSeeder(
                _host!.Thost.Services.GetRequiredService<IUserRepository>(),
                _host.Thost.Services.GetRequiredService<IPasswordHasher<User>>()
            );
            User user = await userSeeder.SeedUser(Guid.NewGuid());
            UpdateUserProfileCommand command = new UpdateUserProfileCommand("ethanDemo", 50, "60128272851");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [TestMethod]
        public async Task UpdateUser_Should_Failed_FluentValidationErrorFullName()
        {
            UserSeeder userSeeder = new UserSeeder(
               _host!.Thost.Services.GetRequiredService<IUserRepository>(),
               _host.Thost.Services.GetRequiredService<IPasswordHasher<User>>()
           );
            User user = await userSeeder.SeedUser();
            UpdateUserProfileCommand command = new UpdateUserProfileCommand("ethan 123", 50, "60128272851");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "FullName"));
        }


        [TestMethod]
        public async Task UpdateUser_Should_Failed_FluentValidationErrorAge()
        {
            UserSeeder userSeeder = new UserSeeder(
               _host!.Thost.Services.GetRequiredService<IUserRepository>(),
               _host.Thost.Services.GetRequiredService<IPasswordHasher<User>>()
           );
            User user = await userSeeder.SeedUser();
            UpdateUserProfileCommand command = new UpdateUserProfileCommand("Ethan Pang", 101, "60128272851");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "Age"));
        }


        [TestMethod]
        public async Task UpdateUser_Should_Failed_FluentValidationErrorPhineNumber()
        {
            UserSeeder userSeeder = new UserSeeder(
               _host!.Thost.Services.GetRequiredService<IUserRepository>(),
               _host.Thost.Services.GetRequiredService<IPasswordHasher<User>>()
           );
            User user = await userSeeder.SeedUser();
            UpdateUserProfileCommand command = new UpdateUserProfileCommand("Ethan Pang", 50, "53210128272851321421421");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "PhoneNumber"));
        }

    }
}

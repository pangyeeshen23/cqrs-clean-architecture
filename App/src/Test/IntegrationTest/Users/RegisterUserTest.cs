using System;
using System.Collections.Generic;
using System.Text;
using Application.Users.Commands.RegisterUser;
using Domain.Exceptions;
using Domain.Exceptions.Users;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Test.Core.Seeder;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Test.IntegrationTest.Users
{
    [TestClass]
    public class RegisterUserTest : BaseIntegrationTest
    {
        private IMediator? _mediator;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            _mediator = _host?.Thost.Services.GetRequiredService<IMediator>();
        }

        [TestMethod]
        public async Task RegisterUser_Should_Success()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang@gamil.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", 23, "0122792350");
            RegisterUserResponse resp = await _mediator!.Send(command);
            Assert.IsNotEmpty(resp.Token);
        }

        [TestMethod]
        public async Task RegisterUser_Should_Failed_UserAlreadyExistsException()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang@gamil.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", 23, "0122792350");
            RegisterUserResponse resp = await _mediator!.Send(command);
            Assert.IsNotEmpty(resp.Token);
            RegisterUserCommand secondCommand = new RegisterUserCommand("ethanPang", "ethanPang@gamil.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", 23, "0122792350");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<UserAlreadyExistsException>();
        }

    }
}

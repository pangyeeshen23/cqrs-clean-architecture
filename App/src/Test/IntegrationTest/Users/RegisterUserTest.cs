using System;
using System.Collections.Generic;
using System.Text;
using Application.Users.Commands.RegisterUser;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Users;
using Domain.Repositories;
using Domain.Repositories.Model.Users;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Repositories;
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
        private IUserRepository? _userRepository;

        [TestInitialize]
        public override async Task TestSetup()
        {
            await base.TestSetup();
            _mediator = _scope!.ServiceProvider.GetRequiredService<IMediator>();
            _userRepository = _scope!.ServiceProvider.GetRequiredService<IUserRepository>();
        }

        [TestMethod]
        public async Task RegisterUser_Should_Success()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang@gamil.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", 23, "60122792350");
            RegisterUserResponse resp = await _mediator!.Send(command);
            UserFilterModel filter = new UserFilterModel();
            filter.Username = "ethanPang";
            filter.IsIncludeUserProfile = true;
            User? user = await _userRepository!.GetByAsync(filter);
            Assert.IsNotEmpty(resp.Token);
            Assert.IsNotNull(user);
            Assert.AreEqual("ethanPang", user.Username);
            Assert.AreEqual("ethanPang@gamil.com", user.Email);
            Assert.AreEqual("Pang Yee Shen", user.Profile.FullName);
            Assert.AreEqual(23, user.Profile.Age);
            Assert.AreEqual("60122792350", user.Profile.PhoneNumber);
        }

        [TestMethod]
        public async Task RegisterUser_Should_Failed_UserAlreadyExistsException()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang@gamil.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", 23, "60122792350");
            RegisterUserResponse resp = await _mediator!.Send(command);
            Assert.IsNotEmpty(resp.Token);
            RegisterUserCommand secondCommand = new RegisterUserCommand("ethanPang", "ethanPang@gamil.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", 23, "0122792350");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<UserAlreadyExistsException>();
        }

        [TestMethod]
        public async Task RegisterUser_Should_Failed_FluentValidationErrorUsername()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang123!@34()", "ethanPang@gamil.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", 23, "0122792350");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "Username"));
        }

        [TestMethod]
        public async Task RegisterUser_Should_Failed_FluentValidationErrorEmail()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang-gmail.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", 23, "0122792350");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "Email"));
        }

        [TestMethod]
        public async Task RegisterUser_Should_Failed_FluentValidationFullName()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang@gmail.com", "Pang Yee Shen !!()", "!root123Qwe123", "!root123Qwe123", 23, "0122792350");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "FullName"));
        }

        [TestMethod]
        public async Task RegisterUser_Should_Failed_FluentValidationPassword()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang@gmail.com", "Pang Yee Shen", "root", "root", 23, "0122792350");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "Password"));
        }

        [TestMethod]
        public async Task RegisterUser_Should_Failed_FluentValidationConfirmPassword()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang@gmail.com", "Pang Yee Shen", "!root123Qwe123", "root", 23, "0122792350");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "ConfirmPassword"));
        }

        [TestMethod]
        public async Task RegisterUser_Should_Failed_FluentValidationAge()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang@gmail.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", -1, "0122792350");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "Age"));
        }


        [TestMethod]
        public async Task RegisterUser_Should_Failed_FluentValidationPhoneNumber()
        {
            RegisterUserCommand command = new RegisterUserCommand("ethanPang", "ethanPang@gmail.com", "Pang Yee Shen", "!root123Qwe123", "!root123Qwe123", 23, "-10122792350");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "PhoneNumber"));
        }
    }
}

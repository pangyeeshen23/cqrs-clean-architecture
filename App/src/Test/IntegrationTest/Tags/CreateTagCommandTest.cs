using System;
using System.Collections.Generic;
using System.Text;
using Application.Tags.Commnads.CreateTag;
using Application.Tags.Queries.GetAllTags;
using Application.Users.Queries.GetUserProfile;
using Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Test.Core.Seeder;

namespace Test.IntegrationTest.Tags
{
    [TestClass]
    public class CreateTagCommandTest : BaseIntegrationTest
    {
        private IMediator? _mediator;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            _mediator = _host?.Thost.Services.GetRequiredService<IMediator>();
        }

        [TestMethod]
        public async Task CreateTag_Should_Success()
        {
            CreateTagCommand command = new CreateTagCommand("Funny", "This is to descript the book as a funny material");
            CreateTagResponse resp = await _mediator!.Send(command);
            Assert.AreNotEqual(resp.Id, Guid.Empty);
        }

        [TestMethod]
        public async Task CreateTag_Should_Fail_FluentExceptionTitle()
        {
            CreateTagCommand command = new CreateTagCommand("Funny Money HAAHAHAHAAHAHAHAHAH", "This is to descript the book as a funny material");
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "Title"));
        }
    }
}

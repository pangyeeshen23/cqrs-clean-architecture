using System;
using System.Collections.Generic;
using System.Text;
using Application.Tags.Commnads.DeleteTag;
using Application.Tags.Commnads.UpdateTag;
using Azure;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Test.Core.Seeder;

namespace Test.IntegrationTest.Tags
{
    [TestClass]
    public class UpdateTagCommandTest : BaseIntegrationTest
    {
        private IMediator? _mediator;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            _mediator = _host?.Thost.Services.GetRequiredService<IMediator>();
        }

        [TestMethod]
        public async Task UpdateTag_Should_Success()
        {
            TagSeeder tagSeeder = new TagSeeder(
               _host!.Thost.Services.GetRequiredService<ITagRepository>()
            );
            Tag tag = await tagSeeder.SeedTag();
            UpdateTagCommand commnad = new UpdateTagCommand(tag.Id, "Kids", "This tag is for a kid tagging");
            await _mediator!.Send(commnad);
        }


        [TestMethod]
        public async Task UpdateTag_Should_Fail_NotFoundException()
        {
            UpdateTagCommand commnad = new UpdateTagCommand(Guid.NewGuid(), "Funny", "This tag is for a kid tagging");
            Func<Task> act = () => _mediator!.Send(commnad);
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [TestMethod]
        public async Task UpdateTag_Should_Fail_FluentValidationId()
        {
            TagSeeder tagSeeder = new TagSeeder(
               _host!.Thost.Services.GetRequiredService<ITagRepository>()
            );
            Tag tag = await tagSeeder.SeedTag();
            UpdateTagCommand commnad = new UpdateTagCommand(Guid.Empty, "Funny", "This tag is for a kid tagging");
            Func<Task> act = () => _mediator!.Send(commnad);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "Id"));
        }

        [TestMethod]
        public async Task UpdateTag_Should_Fail_FluentValidationTitle()
        {
            TagSeeder tagSeeder = new TagSeeder(
               _host!.Thost.Services.GetRequiredService<ITagRepository>()
            );
            Tag tag = await tagSeeder.SeedTag();
            UpdateTagCommand commnad = new UpdateTagCommand(tag.Id, "Funny Money HAAHAHAHAAHAHAHAHAH", "This tag is for a kid tagging");
            Func<Task> act = () => _mediator!.Send(commnad);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "Title"));
        }
    }
}

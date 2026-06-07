using System;
using System.Collections.Generic;
using System.Text;
using Application.Tags.Commnads.DeleteTag;
using Application.Tags.Queries.GetAllTags;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Model.Tags;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Test.Core.Seeder;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Test.IntegrationTest.Tags
{
    [TestClass]
    public class DeleteTagCommnadTest : BaseIntegrationTest
    {
        private IMediator? _mediator;
        private ITagRepository? _tagRepository;

        [TestInitialize]
        public override async Task TestSetup()
        {
            await base.TestSetup();
            _mediator = _scope!.ServiceProvider.GetRequiredService<IMediator>();
            _tagRepository = _scope!.ServiceProvider.GetRequiredService<ITagRepository>();
        }

        [TestMethod]
        public async Task DeleteTag_Should_Success()
        {
            UserSeeder userSeeder = new UserSeeder(
              _scope!.ServiceProvider.GetRequiredService<IUserRepository>(),
              _scope!.ServiceProvider.GetRequiredService<IPasswordHasher<User>>()
            );
            User user = await userSeeder.SeedUser();
            TagSeeder tagSeeder = new TagSeeder(
               _scope!.ServiceProvider.GetRequiredService<ITagRepository>()
            );
            Tag tag = await tagSeeder.SeedTag(user.Id);
            DeleteTagCommand commnad = new DeleteTagCommand(tag.Id);
            await _mediator!.Send(commnad);
            TagFilterModel filter = new TagFilterModel();
            filter.Id = tag.Id;
            Tag? tag1 = await _tagRepository!.GetAsync(filter);
            Assert.IsNull(tag1);
        }


        [TestMethod]
        public async Task DeleteTag_Should_Fail_NotFoundException()
        {
            UserSeeder userSeeder = new UserSeeder(
              _scope!.ServiceProvider.GetRequiredService<IUserRepository>(),
              _scope!.ServiceProvider.GetRequiredService<IPasswordHasher<User>>()
            );
            User user = await userSeeder.SeedUser();
            TagSeeder tagSeeder = new TagSeeder(
               _scope!.ServiceProvider.GetRequiredService<ITagRepository>()
            );
            Tag tag = await tagSeeder.SeedTag(user.Id);
            DeleteTagCommand command = new DeleteTagCommand(Guid.NewGuid());
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<NotFoundException>();
        }


        [TestMethod]
        public async Task DeleteTag_Should_Fail_FluentValidationId()
        {
            DeleteTagCommand command = new DeleteTagCommand(Guid.Empty);
            Func<Task> act = () => _mediator!.Send(command);
            await act.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(message => message.PropertyName == "Id"));
        }
    }
}

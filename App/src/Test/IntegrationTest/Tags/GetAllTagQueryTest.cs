using System;
using System.Collections.Generic;
using System.Text;
using Application.Tags.Queries.GetAllTags;
using Application.Users.Queries.GetUserProfile;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Test.Core.Seeder;

namespace Test.IntegrationTest.Tags
{
    [TestClass]
    public class GetAllTagQueryTest : BaseIntegrationTest
    {
        private IMediator? _mediator;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            _mediator = _host?.Thost.Services.GetRequiredService<IMediator>();
        }

        [TestMethod]
        public async Task GetAllTag_Should_Success()
        {
            TagSeeder tagSeeder = new TagSeeder(
               _host!.Thost.Services.GetRequiredService<ITagRepository>()
           );
            GetAllTagQuery query = new GetAllTagQuery();
            List<GetAllTagResponse> resp = await _mediator!.Send(query);
            foreach (GetAllTagResponse tag in resp)
            {
                Assert.AreNotEqual(Guid.Empty, tag.Id);
                Assert.IsNotEmpty(tag.Title);
                Assert.IsNotEmpty(tag.Description);
                Assert.IsNotEmpty(tag.Slug);
            }
        }
    }
}

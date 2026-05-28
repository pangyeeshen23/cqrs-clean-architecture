using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Authentication;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Test.Core.Seeder
{
    public class TagSeeder
    {
        private readonly ITagRepository _tagRepository;

        public TagSeeder(ITagRepository repository)
        {
            _tagRepository = repository;
        }

        public async Task<Tag> SeedTag()
        {
            Tag tag = new Tag
            {
                Title = "Adventure",
                Description = "This tag descript the book is an adventure book",
                Slug = "adventure"
            };
            await _tagRepository.CreateAsync(tag);
            return tag;
        }
    }
}

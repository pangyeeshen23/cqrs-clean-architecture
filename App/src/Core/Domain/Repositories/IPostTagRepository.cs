using Domain.Entities;
using Domain.Repositories.Model.Posts;
using Domain.Repositories.Model.PostTags;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories
{
    public interface IPostTagRepository
    {
        public Task<List<PostTags>> GetAllByAync(PostTagsFilterModel filter);
        public Task CreateRangeAsync(List<PostTags> postTags);
        public Task DeleteRangeAsync(List<PostTags> postTags);
    }
}

using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.PostTags;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Repositories
{
    public class PostTagRepository : IPostTagRepository
    {
        private readonly MyDbContext _dbContext;

        public PostTagRepository(
            MyDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task<List<PostTags>> GetAllByAync(PostTagsFilterModel filter)
        {
            IQueryable<PostTags> query = _dbContext.PostTags.AsQueryable();
            ApplyPostIdFitter(filter.PostId, ref query);
            return await query.ToListAsync();
        }

        private void ApplyPostIdFitter(Guid? id, ref IQueryable<PostTags> query)
        {
            if (id.HasValue)
            {
                query = query.Where(t => t.PostId == id.Value);
            }

        }

        public async Task CreateRangeAsync(List<PostTags> postTags)
        {
            await _dbContext.PostTags.AddRangeAsync(postTags);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<PostTags> postTags)
        {
            _dbContext.PostTags.RemoveRange(postTags);
            await _dbContext.SaveChangesAsync();
        }
    }
}

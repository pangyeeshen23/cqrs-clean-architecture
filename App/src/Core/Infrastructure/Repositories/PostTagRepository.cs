using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.PostTags;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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

        public async Task CreateAsync(Post post)
        {
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post post)
        {
            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();
        }
    }
}

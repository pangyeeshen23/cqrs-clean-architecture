using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.Posts;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly MyDbContext _dbContext;

        public PostRepository(
            MyDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetAllByAync(PostFilterModel filter)
        {
            IQueryable<Post> query = _dbContext.Posts.AsQueryable();
            ApplyIdFitter(filter.Id, ref query);
            ApplyOwnerIdFitter(filter.OwnerId, ref query);
            if (filter.IncludeTags) IncludeTags(ref query);
            return await query.ToListAsync();
        }

        public async Task<Post?> GetAsync(PostFilterModel filter)
        {
            IQueryable<Post> query = _dbContext.Posts.AsQueryable();
            ApplyIdFitter(filter.Id, ref query);
            ApplyOwnerIdFitter(filter.OwnerId, ref query);
            if (filter.IncludeTags) IncludeTags(ref query);
            return await query.FirstOrDefaultAsync();
        }

        private void IncludeTags(ref IQueryable<Post> query)
        {
            query = query.Include(e => e.PostTags).ThenInclude(e => e.Tag);
        }

        private void ApplyIdFitter(Guid? id, ref IQueryable<Post> query)
        {
            if (id.HasValue)
            {
                query = query.Where(t => t.Id == id.Value);
            }

        }

        private void ApplyOwnerIdFitter(Guid? ownerId, ref IQueryable<Post> query)
        {
            if (ownerId.HasValue)
            {
                query = query.Where(t => t.OwnerId == ownerId.Value);
            }
        }

        public async Task CreateAsync(Post post)
        {
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            _dbContext.Posts.Update(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post post)
        {
            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<Post> posts)
        {
            _dbContext.Posts.RemoveRange(posts);
            await _dbContext.SaveChangesAsync();
        }
    }
}

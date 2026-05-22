using Domain.Entities;
using Domain.Repositories.Model.Posts;

namespace Domain.Repositories
{
    public interface IPostRepository
    {
        public Task<Post?> GetAsync(PostFilterModel filter);
        public Task<List<Post>> GetAllByAync(PostFilterModel filter);
        public Task CreateAsync(Post post);
        public Task UpdateAsync(Post post);
        public Task DeleteAsync(Post post);
    }
}

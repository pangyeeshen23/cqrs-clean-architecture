using Domain.Entities;
using Domain.Repositories.Model.Tags;

namespace Domain.Repositories
{
    public interface ITagRepository
    {
        public Task<Tag?> GetAsync(TagFilterModel filter);
        public Task<List<Tag>> GetAllByAync(TagFilterModel? filter = null);
        public Task CreateAsync(Tag tag);
        public Task UpdateAsync(Tag tag);
        public Task DeleteAsync(Tag tag);
    }
}

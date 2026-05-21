using Domain.Entities;

namespace Application.Handler
{
    public interface IUserHandler
    {
        public Task<List<User>> GetUsers();
    }
}

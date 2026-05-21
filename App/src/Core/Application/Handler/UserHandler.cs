using Domain.Entities;
using Domain.Repositories;

namespace Application.Handler
{
    public class UserHandler : IUserHandler
    {
        private readonly IUserRepository _messageRepository;
        public UserHandler(IUserRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<List<User>> GetUsers()
        {
            return await _messageRepository.GetAllAsync();
        }
    }
}

using Domain.Entities;
using Domain.Repositories.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByAsync(string username, string email);
        Task<User?> GetByAsync(UserFilterModel filter);
        Task AddAsync(User user);
    }
}

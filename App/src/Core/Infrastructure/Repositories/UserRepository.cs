using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _dbContext;
        public UserRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public Task<User?> GetByAsync(UserFilterModel filter)
        {
            IQueryable<User> query = _dbContext.Users.AsQueryable();
            ApplyUsernameFilter(filter.Username, ref query);
            ApplyEmailFilter(filter.Email, ref query);
            return query.FirstOrDefaultAsync();
        }

        private void ApplyUsernameFilter(string? username, ref IQueryable<User> query)
        {
            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(u => u.Username == username);
            }
        }

        private void ApplyEmailFilter(string? email, ref IQueryable<User> query)
        {
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email == email);
            }
        }

        public Task<User?> GetByAsync(string username, string email)
        {
            return _dbContext.Users.Where(e => e.Username == username || e.Email == email).FirstOrDefaultAsync();
        }
    }
}

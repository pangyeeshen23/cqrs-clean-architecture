using System.Data;
using System.Diagnostics;
using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.Users;
using Infrastructure.Context;
using Infrastructure.Contexts.Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _dbContext;
        private readonly DapperContext _dapperContext;
        private readonly ILogger<UserProfileRepository> _logger;

        public UserRepository(MyDbContext dbContext, DapperContext dapperContext, ILogger<UserProfileRepository> logger)
        {
            _dbContext = dbContext;
            _dapperContext = dapperContext;
            _logger = logger;
        }

        public async Task<List<User>> GetAllAsync(UserFilterModel filter)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<User> users = new List<User>();
            if (filter.UseDapper)
            {
                using IDbConnection connection = _dapperContext.CreateConnection();
                users = (await connection.QueryAsync<User>("SELECT * FROM Users")).ToList();
            }
            else users = await _dbContext.Users.ToListAsync();
            stopwatch.Stop();
            _logger.LogInformation("GetAsync executed in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
            return users;
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
            if (filter.IsIncludeUserProfile) IncludeUserProfile(ref query);
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

        private void IncludeUserProfile(ref IQueryable<User> query)
        {
            query = query.Include(e => e.Profile);
        }

        public Task<User?> GetByAsync(string username, string email)
        {
            return _dbContext.Users.Where(e => e.Username == username || e.Email == email).FirstOrDefaultAsync();
        }
    }
}

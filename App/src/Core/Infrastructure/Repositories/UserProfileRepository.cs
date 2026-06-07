using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.UserProfiles;
using Infrastructure.Context;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly MyDbContext _dbContext;
        private readonly DapperContext _dapperContext;
        private readonly ILogger<UserProfileRepository> _logger;

        public UserProfileRepository(
            MyDbContext dbContext,
            DapperContext dapperContext,
            ILogger<UserProfileRepository> logger)
        {
            _dbContext = dbContext;
            _dapperContext = dapperContext;
            _logger = logger;
        }

        public async Task<UserProfile?> GetAsync(UserProfileFilterModel filter)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            UserProfile? userProfile = null;
            if (!filter.UseDapper)
            {
                IQueryable<UserProfile> query = _dbContext.UserProfiles.AsQueryable();
                ApplyUserIdFilter(filter.UserId, ref query);
                userProfile = await query.FirstOrDefaultAsync();
            }
            else
            {
                using var dbContext = _dapperContext.CreateConnection();
                userProfile = await dbContext.QueryFirstAsync<UserProfile>("SELECT * FROM UserProfiles WHERE UserId = @UserId", new { UserId = filter.UserId });
            }
            stopwatch.Stop();
            _logger.LogInformation("GetAsync executed in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
            return userProfile;
        }

        private void ApplyUserIdFilter(Guid? userId, ref IQueryable<UserProfile> query)
        {
            if (userId.HasValue)
            {
                query = query.Where(up => up.UserId == userId.Value);
            }
        }

        public async Task UpdateAsync(UserProfile profile)
        {
            _dbContext.Update(profile);
            await _dbContext.SaveChangesAsync();
        }
    }
}

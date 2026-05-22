using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.UserProfiles;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly MyDbContext _dbContext;
        public UserProfileRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfile?> GetAsync(UserProfileFilterModel filter)
        {
            IQueryable<UserProfile> query = _dbContext.UserProfiles.AsQueryable();
            ApplyUserIdFilter(filter.UserId, ref query);
            return await query.FirstOrDefaultAsync();
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

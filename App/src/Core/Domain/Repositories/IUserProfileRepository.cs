using Domain.Entities;
using Domain.Repositories.Model.UserProfiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories
{
    public interface IUserProfileRepository
    {
        public Task<UserProfile?> GetAsync(UserProfileFilterModel filter);
        public Task UpdateAsync(UserProfile profile);
    }
}

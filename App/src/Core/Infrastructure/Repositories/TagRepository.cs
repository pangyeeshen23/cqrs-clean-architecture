using Azure;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.Tags;
using Domain.Repositories.Model.UserProfiles;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {

        private readonly MyDbContext _dbContext;

        public TagRepository(
            MyDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task<List<Tag>> GetAllByAync(TagFilterModel? filter = null)
        {
            IQueryable<Tag> query = _dbContext.Tags.AsQueryable();
            ApplyIdFitter(filter?.Id, ref query);
            ApplyOwnerIdFitter(filter?.OwnerId, ref query);
            return await query.ToListAsync();
        }

        public async Task<Tag?> GetAsync(TagFilterModel filter)
        {
            IQueryable<Tag> query = _dbContext.Tags.AsQueryable();
            ApplyIdFitter(filter.Id, ref query);
            return await query.FirstOrDefaultAsync();
        }

        private void ApplyIdFitter(Guid? id, ref IQueryable<Tag> query)
        {
            if (id.HasValue)
            {
                query = query.Where(t => t.Id == id.Value);
            }

        }

        private void ApplyOwnerIdFitter(Guid? ownerId, ref IQueryable<Tag> query)
        {
            if (ownerId.HasValue)
            {
                query = query.Where(t => t.OwnerId == ownerId.Value);
            }
        }

        public async Task CreateAsync(Tag tag)
        {
            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag tag)
        {
            _dbContext.Tags.Update(tag);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tag tag)
        {
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();
        }
    }
}

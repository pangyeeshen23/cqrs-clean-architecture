using Application.Common.Interfaces;
using Application.Common.Redis;
using Domain.Caching;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Model.Tags;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tags.Commnads.UpdateTag
{
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;

        public UpdateTagCommandHandler(
            ITagRepository tagRepository,
            ICurrentUserService currentUserService,
            ICacheService cacheService
        )
        {
            _tagRepository = tagRepository;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }

        public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            TagFilterModel filter = new TagFilterModel();
            filter.Id = request.Id;
            filter.OwnerId = _currentUserService.UserId;
            Tag tag = await _tagRepository.GetAsync(filter) ?? throw new NotFoundException("Tag");
            if(!string.IsNullOrEmpty(request.Title))
            {
                tag.Title = request.Title;
                tag.Slug = request.Title.ToLower().Trim().Replace(" ", "-");
            }
            tag.Description = request.Description;
            await _cacheService.RemoveAsync($"{RedisKeys.TagList}", cancellationToken);
            await _tagRepository.UpdateAsync(tag);
        }
    }
}

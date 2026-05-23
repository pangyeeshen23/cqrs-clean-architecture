using AngleSharp.Io;
using Application.Common.Interfaces;
using Application.Common.Redis;
using Domain.Caching;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.Tags;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tags.Commnads.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, CreateTagResponse>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;

        public CreateTagCommandHandler(
            ITagRepository tagRepository,
            ICurrentUserService currentUserService,
            ICacheService cacheService
        )
        {
            _tagRepository = tagRepository;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }

        public async Task<CreateTagResponse> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            Tag tag = new Tag
            {
                Title = request.Title,
                Description = request.Description,
                Slug = request.Title.ToLower().Trim().Replace(" ", "-"),
                OwnerId = _currentUserService.UserId
            };
            await _tagRepository.CreateAsync(tag);
            await _cacheService.RemoveAsync($"{RedisKeys.TagList}", cancellationToken);
            return new CreateTagResponse(tag.Id);
        }
    }
}

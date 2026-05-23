using Application.Common.Interfaces;
using Application.Tags.Commnads.CreateTag;
using Application.Tags.Common.Redis;
using Domain.Caching;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Model.Tags;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tags.Commnads.DeleteTag
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;

        public DeleteTagCommandHandler(
            ITagRepository tagRepository,
            ICurrentUserService currentUserService,
            ICacheService cacheService
        )
        {
            _tagRepository = tagRepository;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            TagFilterModel filter = new TagFilterModel();
            filter.Id = request.Id;
            filter.OwnerId = _currentUserService.UserId;
            Tag tag = await _tagRepository.GetAsync(filter) ?? throw new NotFoundException("Tag");
            await _cacheService.RemoveAsync($"{RedisKeys.List}", cancellationToken);
            await _tagRepository.DeleteAsync(tag);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Redis;
using Domain.Caching;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Model.Posts;
using MediatR;

namespace Application.Posts.Commands.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;

        public DeletePostCommandHandler(
            IPostRepository postRepository,
            ICurrentUserService currentUserService,
            ICacheService cacheService
        )
        {
            _postRepository = postRepository;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }

        public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            PostFilterModel filter = new PostFilterModel();
            filter.Id = request.Id;
            filter.OwnerId = _currentUserService.UserId;
            Post post = await _postRepository.GetAsync(filter) ?? throw new NotFoundException("Post");
            await _postRepository.DeleteAsync(post);
            string key = RedisKeys.PostWildCard.Replace("{user_id}", _currentUserService.UserId.ToString());
            await _cacheService.RemoveItemsByPatternAsync(key);
        }
    }
}

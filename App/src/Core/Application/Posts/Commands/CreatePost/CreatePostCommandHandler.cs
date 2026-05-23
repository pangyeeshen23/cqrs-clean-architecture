using Application.Common.Interfaces;
using Application.Posts.Common.Redis;
using Domain.Caching;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand,  CreatePostResponse>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;
        public CreatePostCommandHandler(
            IPostRepository postRepository,
            ICurrentUserService currentUserService,
            ICacheService cacheService
        )
        {
            _postRepository = postRepository;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }

        public async Task<CreatePostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            List<PostTags> tags = request.Tags.Select(e => new PostTags() { TagId = e}).ToList();
            Post post = new Post()
            {
                Title = request.Title,
                Content = request.Content,
                OwnerId = _currentUserService.UserId,
                PostTags = tags
            };
            await _postRepository.CreateAsync(post);
            await _cacheService.RemoveAsync($"{RedisKeys.List}", cancellationToken);
            return new CreatePostResponse(post.Id);
        }
    }
}

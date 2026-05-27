using System;
using System.Collections.Generic;
using System.Text;
using AngleSharp.Common;
using Application.Common.Interfaces;
using Application.Common.Redis;
using Domain.Caching;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Model.Posts;
using MediatR;

namespace Application.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, UpdatePostResponse>
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;

        public UpdatePostCommandHandler(
            IPostRepository postRepository,
            ICurrentUserService currentUserService,
            IPostTagRepository postTagRepository,
            ICacheService cacheService
        )
        {
            _postRepository = postRepository;
            _currentUserService = currentUserService;
            _postTagRepository = postTagRepository;
            _cacheService = cacheService;
        }


        public async Task<UpdatePostResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            PostFilterModel filter = new PostFilterModel();
            filter.Id = request.Id;
            filter.OwnerId = _currentUserService.UserId;
            filter.IncludeTags = true;
            Post post = await _postRepository.GetAsync(filter) ?? throw new NotFoundException("Post");
            if (string.IsNullOrWhiteSpace(request.Title)) post.Title = request.Title;
            if (string.IsNullOrWhiteSpace(request.Content)) post.Content = request.Content;
            await _postRepository.UpdateAsync(post);
            List<Guid> existingPostTags = post.PostTags.Select(e => e.TagId).ToList();
            List<Guid> addedTagIds = request.TagIds.Except(existingPostTags).ToList();
            List<PostTags> removeTagIds = post.PostTags.Where(o => !request.TagIds.Any(n => n == o.TagId)).ToList();
            List<PostTags> addedTags = addedTagIds.Select(e => new PostTags() { PostId = post.Id, TagId = e }).ToList();
            await _postTagRepository.DeleteRangeAsync(removeTagIds);
            await _postTagRepository.CreateRangeAsync(addedTags);
            string key = RedisKeys.PostWildCard.Replace("{user_id}", _currentUserService.UserId.ToString());
            await _cacheService.RemoveItemsByPatternAsync(key);
            return new UpdatePostResponse(post.Id);
        }
    }
}

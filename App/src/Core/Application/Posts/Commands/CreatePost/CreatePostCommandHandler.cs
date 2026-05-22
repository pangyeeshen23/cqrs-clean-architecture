using Application.Common.Interfaces;
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
        public CreatePostCommandHandler(
            IPostRepository postRepository,
            ICurrentUserService currentUserService
        )
        {
            _postRepository = postRepository;
            _currentUserService = currentUserService;
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
            return new CreatePostResponse(post.Id);
        }
    }
}

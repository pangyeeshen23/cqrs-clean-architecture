using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Model.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Posts.Commands.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUserService _currentUserService;
        public DeletePostCommandHandler(
            IPostRepository postRepository,
            ICurrentUserService currentUserService
        )
        {
            _postRepository = postRepository;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            PostFilterModel filter = new PostFilterModel();
            filter.Id = request.Id;
            filter.OwnerId = _currentUserService.UserId;
            Post post = await _postRepository.GetAsync(filter) ?? throw new NotFoundException("Post");
            await _postRepository.DeleteAsync(post);
        }
    }
}

using Application.Common.Interfaces;
using Application.Posts.Commands.CreatePost;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.Posts;
using Ganss.Xss;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Posts.Queries.GetAllPosts
{
    public class GetAllPostQueryHandler : IRequestHandler<GetAllPostQuery, List<GetAllPostResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAllPostQueryHandler(
            IPostRepository postRepository,
            ICurrentUserService currentUserService
        )
        {
            _postRepository = postRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<GetAllPostResponse>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            PostFilterModel filter = new PostFilterModel();
            filter.OwnerId = _currentUserService.UserId;
            filter.IncludeTags = true;
            List<Post> posts = await _postRepository.GetAllByAync(filter);
            var sanitizer = new HtmlSanitizer();
            List<GetAllPostResponse> response = posts.Select(e => 
                new GetAllPostResponse(
                    e.Id, 
                    sanitizer.Sanitize(e.Title), 
                    sanitizer.Sanitize(e.Content), 
                    e.PostTags.Select(e => new TagResponse(e.Tag.Id, sanitizer.Sanitize(e.Tag.Title)))
                )
           ).ToList();
            return response;
        }
    }
}

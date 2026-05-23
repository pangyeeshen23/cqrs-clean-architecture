using Application.Common.Interfaces;
using Application.Common.Redis;
using Domain.Caching;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.Posts;
using Ganss.Xss;
using MediatR;

namespace Application.Posts.Queries.GetAllPosts
{
    public class GetAllPostQueryHandler : IRequestHandler<GetAllPostQuery, List<GetAllPostResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;

        public GetAllPostQueryHandler(
            IPostRepository postRepository,
            ICurrentUserService currentUserService,
            ICacheService cacheService
        )
        {
            _postRepository = postRepository;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }

        public async Task<List<GetAllPostResponse>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            string key = RedisKeys.PostList.Replace("{user_id}", _currentUserService.UserId.ToString());
            List<GetAllPostResponse>? cached = await _cacheService.GetAsync<List<GetAllPostResponse>>(key, cancellationToken);
            if (cached != null) return cached;
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
            await _cacheService.SetAsync(key, response, TimeSpan.FromHours(1), cancellationToken);
            return response;
        }
    }
}

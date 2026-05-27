using System.Text.Json;
using Application.Common.Hasher;
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
            string md5 = MD5Hasher.ToMd5(JsonSerializer.Serialize(request));
            string key = RedisKeys.PostList.Replace("{user_id}", _currentUserService.UserId.ToString());
            key = key.Replace("{param}", md5);
            List<GetAllPostResponse>? cached = await _cacheService.GetAsync<List<GetAllPostResponse>>(key, cancellationToken);
            if (cached != null) return cached;
            PostFilterModel filter = new PostFilterModel();
            filter.Title = request.Title;
            filter.Content = request.Content;
            filter.OwnerId = _currentUserService.UserId;
            filter.IncludeTags = true;
            filter.PageSize = request.PageSize;
            filter.Page = request.Page;
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

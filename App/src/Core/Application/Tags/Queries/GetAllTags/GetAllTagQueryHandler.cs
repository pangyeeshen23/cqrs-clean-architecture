using Application.Common.Interfaces;
using Application.Tags.Common.Redis;
using Domain.Caching;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.Tags;
using Ganss.Xss;
using MediatR;

namespace Application.Tags.Queries.GetAllTags
{
    public class GetAllTagQueryHandler : IRequestHandler<GetAllTagQuery, List<GetAllTagResponse>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICacheService _cacheService;

        public GetAllTagQueryHandler(
            ITagRepository tagRepository,
            ICurrentUserService currentUserService,
            ICacheService cacheService
        )
        {
            _tagRepository = tagRepository;
            _cacheService = cacheService;
        }

        public async Task<List<GetAllTagResponse>> Handle(GetAllTagQuery request, CancellationToken cancellationToken)
        {
            List<GetAllTagResponse>? cached = await _cacheService.GetAsync<List<GetAllTagResponse>>($"{RedisKeys.List}", cancellationToken);
            if (cached != null) return cached;
            List<Tag> tags = await _tagRepository.GetAllByAync();
            var sanitizer = new HtmlSanitizer();
            List<GetAllTagResponse> response = tags.Select(
                t => new GetAllTagResponse(t.Id, sanitizer.Sanitize(t.Title), sanitizer.Sanitize(t.Description), sanitizer.Sanitize(t.Slug))
            ).ToList();
            await _cacheService.SetAsync($"{RedisKeys.List}", response, TimeSpan.FromHours(1), cancellationToken);
            return response;
        }
    }
}

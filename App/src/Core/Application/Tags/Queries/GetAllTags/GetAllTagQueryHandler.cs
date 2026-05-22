using Application.Common.Interfaces;
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
        private readonly ICurrentUserService _currentUserService;
        public GetAllTagQueryHandler(
            ITagRepository tagRepository,
            ICurrentUserService currentUserService
        )
        {
            _tagRepository = tagRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<GetAllTagResponse>> Handle(GetAllTagQuery request, CancellationToken cancellationToken)
        {
            List<Tag> tags = await _tagRepository.GetAllByAync();
            var sanitizer = new HtmlSanitizer();
            List<GetAllTagResponse> response = tags.Select(
                t => new GetAllTagResponse(t.Id, sanitizer.Sanitize(t.Title), sanitizer.Sanitize(t.Description), sanitizer.Sanitize(t.Slug))
            ).ToList();
            return response;
        }
    }
}

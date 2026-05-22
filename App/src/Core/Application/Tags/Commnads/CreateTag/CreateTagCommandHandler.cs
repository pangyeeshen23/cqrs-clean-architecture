using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Model.Tags;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Tags.Commnads.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, CreateTagResponse>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateTagCommandHandler(
            ITagRepository tagRepository,
            ICurrentUserService currentUserService
        )
        {
            _tagRepository = tagRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CreateTagResponse> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            Tag tag = new Tag
            {
                Title = request.Title,
                Description = request.Description,
                Slug = request.Title.ToLower().Trim().Replace(" ", "-"),
                OwnerId = _currentUserService.UserId
            };
            await _tagRepository.CreateAsync(tag);
            return new CreateTagResponse(tag.Id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Application.Users.Queries.GetUserProfile;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Model.Users;
using MediatR;

namespace Application.Users.Queries.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, IEnumerable<GetUserListResponse>>
    {
        private readonly IUserRepository _userRepository;
        public GetUserListQueryHandler(
            IUserRepository userRepository
        )
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<GetUserListResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            UserFilterModel filter = new UserFilterModel();
            filter.UseDapper = true;
            List<User> users = await _userRepository.GetAllAsync(filter) ?? throw new NotFoundException("User");
            return users.Select(u => new GetUserListResponse(u.Username, u.Email)).ToList();
        }
    }
}

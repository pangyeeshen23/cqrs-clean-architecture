using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Authentication
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(Guid userId, string email, string role);

    }
}

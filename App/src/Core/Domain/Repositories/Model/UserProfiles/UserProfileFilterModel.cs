using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories.Model.UserProfiles
{
    public class UserProfileFilterModel
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
    }
}

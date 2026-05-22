using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories.Model.Posts
{
    public class PostFilterModel
    {
        public Guid? Id { get; set;  }
        public Guid? OwnerId { get; set; }
        public bool IncludeTags { get; set; }

    }
}

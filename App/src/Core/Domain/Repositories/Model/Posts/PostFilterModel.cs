using System;
using System.Collections.Generic;
using System.Text;
using Domain.Repositories.Model.Common;

namespace Domain.Repositories.Model.Posts
{
    public class PostFilterModel : PaginatedFilterModel
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid? OwnerId { get; set; }
        public bool IncludeTags { get; set; }

    }
}

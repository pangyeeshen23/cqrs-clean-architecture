using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories.Model.Tags
{
    public class TagFilterModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public Guid? OwnerId { get; set; }
    }
}

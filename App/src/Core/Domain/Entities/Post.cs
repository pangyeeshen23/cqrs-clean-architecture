using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Post : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public User Owner { get; set; } = default!;
        public Guid OwnerId { get; set; }
        public ICollection<PostTags> PostTags { get; set; } = new List<PostTags>();
    }
}

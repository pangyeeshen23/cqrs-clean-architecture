using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Tag : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public User Owner { get; set; } = default!;
        public Guid OwnerId {  get; set; }
        public ICollection<PostTags> PostTags { get; set; } = new List<PostTags>();
    }
}

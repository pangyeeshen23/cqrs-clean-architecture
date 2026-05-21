using System.ComponentModel.DataAnnotations;

namespace webb.Core.Domain
{
    public class Post : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;

        public User Owner { get; set; } = default!;
        public Guid OwnerId { get; set; }

    }
}

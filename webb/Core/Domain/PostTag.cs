using System.ComponentModel.DataAnnotations;

namespace webb.Core.Domain
{
    public class PostTag : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Post Post { get; set; } = default!;
        public Guid PostId { get; set; } = default!;

        public Tag Tag { get; set; } = default!;
        public Guid TagId { get; set; } = default!;
    }
}

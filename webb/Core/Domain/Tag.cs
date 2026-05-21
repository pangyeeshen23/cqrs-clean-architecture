using System.ComponentModel.DataAnnotations;

namespace webb.Core.Domain
{
    public class Tag : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;

    }
}

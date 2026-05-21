using System.ComponentModel.DataAnnotations;

namespace webb.Core.Domain
{
    public class User : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
}

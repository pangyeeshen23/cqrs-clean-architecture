using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class UserProfile : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public User User { get; set; } = default!;
        public Guid UserId { get; set; }
    }
}

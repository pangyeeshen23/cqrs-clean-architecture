using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AuditableEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; }
    }
}

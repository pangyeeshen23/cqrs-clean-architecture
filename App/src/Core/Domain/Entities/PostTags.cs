using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class PostTags
    {
        [Key]
        public Guid Id { get; set; }

        public Post Post { get; set; } = default!;
        public Guid PostId { get; set; }

        public Tag Tag {  get; set; } = default!;
        public Guid TagId { get; set; }
    }
}

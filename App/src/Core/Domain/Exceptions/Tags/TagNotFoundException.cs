using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions.Tags
{
    public class TagNotFoundException : DomainException
    {
        public TagNotFoundException() : base("Tag not found.")
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string entity) : base($"{entity} not found.")
        {
        }
    }
}

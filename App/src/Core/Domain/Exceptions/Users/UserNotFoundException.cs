using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions.Users
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException() : base("User not found.")
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions.Users
{
    public class UserAlreadyExistsException : DomainException
    {
        public UserAlreadyExistsException(string email) : base($"A user with the email '{email}' already exists.")
        {
        }
    }
}

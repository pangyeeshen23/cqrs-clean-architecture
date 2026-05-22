using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions.Users
{
    public class UserAlreadyExistsException : DomainException
    {
        public UserAlreadyExistsException() : base("A user with this usernam/email has been existed")
        {
        }
    }
}

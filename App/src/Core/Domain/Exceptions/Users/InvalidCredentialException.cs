using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions.Users
{
    public class InvalidCredentialException : DomainException
    {
        public InvalidCredentialException() : base("Invalid credentials.")
        {
        }
    }
}

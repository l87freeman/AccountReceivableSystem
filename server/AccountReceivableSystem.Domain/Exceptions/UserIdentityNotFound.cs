using System;

namespace AccountReceivableSystem.Domain.Exceptions;

public class UserIdentityNotFound : Exception
{
    public UserIdentityNotFound() : base("User identity not found")
    {
        
    }
}
namespace IdentityServer.Application.Exceptions;

public class CreateConflictException : Exception
{
    public CreateConflictException() : base("User data is not unique")
    {   
    }
}
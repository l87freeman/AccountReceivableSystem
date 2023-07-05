namespace IdentityServer.Application.Exceptions;

public class IncorrectLoginDataException : Exception
{
    public IncorrectLoginDataException(string type) : base($"Not correct {type}")
    {
    }
}
namespace IdentityServer.Application.Services.Abstractions;

public interface IHashService
{
    string ComputeHash(string stringToHash);

    bool IsMatch(string hashedString, string plainString);
}
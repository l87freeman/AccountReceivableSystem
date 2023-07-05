using System.Security.Cryptography;
using IdentityServer.Application.Services.Abstractions;

namespace IdentityServer.Application.Services;

public class HashService : IHashService
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100;


    public string ComputeHash(string stringToHash)
    {
        using (var algorithm = new Rfc2898DeriveBytes(
                   stringToHash,
                   SaltSize,
                   Iterations,
                   HashAlgorithmName.SHA512))
        {
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{salt}.{key}";
        }
    }

    public bool IsMatch(string hashedString, string plainString)
    {
        var parts = hashedString.Split('.', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
        {
            throw new FormatException("Hashed string has incorrect format");
        }

        var salt = Convert.FromBase64String(parts[0]);
        var key = Convert.FromBase64String(parts[1]);

        using (var algorithm = new Rfc2898DeriveBytes(
                   plainString,
                   salt,
                   Iterations,
                   HashAlgorithmName.SHA512))
        {
            var keyToCheck = algorithm.GetBytes(KeySize);

            var verified = keyToCheck.SequenceEqual(key);

            return verified;
        }
    }
}
using IdentityServer.Application.Entities;
using IdentityServer.Application.Exceptions;
using IdentityServer.Application.Repositories.Abstractions;
using MongoDB.Driver;

namespace IdentityServer.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserIdentity> _userIdentityCollection;

    public UserRepository(IMongoCollection<UserIdentity> userIdentityCollection)
    {
        _userIdentityCollection = userIdentityCollection;
    }

    public async Task<UserIdentity> FindUserAsync(string email, CancellationToken cancellationToken)
    {
        var result = await _userIdentityCollection
            .Find(user => user.Email.ToLowerInvariant() == email.ToLowerInvariant())
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task CreateUserAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        try
        {
            await _userIdentityCollection.InsertOneAsync(user, new InsertOneOptions(), cancellationToken);
        }
        catch (MongoWriteException e) when (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            throw new CreateConflictException();
        }
    }
}
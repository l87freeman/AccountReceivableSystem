using IdentityServer.Application.Entities;
using IdentityServer.Application.Repositories.Abstractions;
using MongoDB.Driver;

namespace IdentityServer.Application.Repositories;

public class UserDatabaseConfigurator : IDatabaseConfigurator
{
    private readonly IMongoCollection<UserIdentity> _userIdentityCollection;

    public UserDatabaseConfigurator(IMongoCollection<UserIdentity> userIdentityCollection)
    {
        _userIdentityCollection = userIdentityCollection;
    }

    async Task IDatabaseConfigurator.ConfigureAsync()
    {
        var uniqueOptions = new CreateIndexOptions { Unique = true };
        var emailIndex = Builders<UserIdentity>.IndexKeys.Descending(x => x.Email);

        await _userIdentityCollection.Indexes.CreateOneAsync(new CreateIndexModel<UserIdentity>(emailIndex, uniqueOptions));
    }
}
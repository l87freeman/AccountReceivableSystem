using IdentityServer.Application.Entities;

namespace IdentityServer.Application.Repositories.Abstractions;

public interface IUserRepository
{
    Task<UserIdentity> FindUserAsync(string email, CancellationToken cancellationToken);

    Task CreateUserAsync(UserIdentity user, CancellationToken cancellationToken);
}
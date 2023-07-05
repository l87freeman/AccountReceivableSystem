using IdentityServer.Application.Entities;

namespace IdentityServer.Application.Services.Abstractions;

public interface IUserService
{
    Task<TokenModel> Login(UserIdentity map, CancellationToken cancellationToken);

    Task RegisterAsync(UserIdentity map, CancellationToken cancellationToken);
}
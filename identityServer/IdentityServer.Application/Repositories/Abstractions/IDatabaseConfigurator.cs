namespace IdentityServer.Application.Repositories.Abstractions;

public interface IDatabaseConfigurator
{
    Task ConfigureAsync();
}
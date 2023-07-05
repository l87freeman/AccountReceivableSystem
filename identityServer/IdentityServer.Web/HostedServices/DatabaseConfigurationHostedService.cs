using IdentityServer.Application.Repositories.Abstractions;

namespace IdentityServer.Web.HostedServices;

public class DatabaseConfigurationHostedService : BackgroundService
{
    private readonly IDatabaseConfigurator _databaseConfig;

    public DatabaseConfigurationHostedService(IDatabaseConfigurator databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _databaseConfig.ConfigureAsync();
    }
}
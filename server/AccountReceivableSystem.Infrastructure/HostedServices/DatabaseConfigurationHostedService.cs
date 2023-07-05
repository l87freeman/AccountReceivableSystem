using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Hosting;

namespace AccountReceivableSystem.Infrastructure.HostedServices;

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
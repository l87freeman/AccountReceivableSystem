using System.Threading.Tasks;

namespace AccountReceivableSystem.Infrastructure.Repositories.Abstractions;

public interface IDatabaseConfigurator
{
    Task ConfigureAsync();
}
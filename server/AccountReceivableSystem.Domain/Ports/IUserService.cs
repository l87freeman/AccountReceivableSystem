using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;

namespace AccountReceivableSystem.Domain.Ports;

public interface IUserService
{
    Task<UserIdentity> GetCurrentUserAsync(CancellationToken cancellationToken);
}
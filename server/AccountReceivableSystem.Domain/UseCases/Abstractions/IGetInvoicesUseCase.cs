using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;

namespace AccountReceivableSystem.Domain.UseCases.Abstractions;

public interface IGetInvoicesUseCase
{
    Task<ICollection<Invoice>> GetAsync(CancellationToken cancellationToken);
}
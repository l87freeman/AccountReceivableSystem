using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;

namespace AccountReceivableSystem.Domain.UseCases.Abstractions;

public interface IUpdateInvoiceUseCase
{
    Task UpdateInvoiceAsync(UpdateInvoice invoice, CancellationToken cancellationToken);
}
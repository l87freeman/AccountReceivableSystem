using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;

namespace AccountReceivableSystem.Domain.UseCases.Abstractions;

public interface ICreateInvoiceUseCase
{
    Task<Invoice> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken);
}
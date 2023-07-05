using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;

namespace AccountReceivableSystem.Application.Services.Abstractions;

public interface IInvoiceService
{
    Task<Invoice> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken);

    Task UpdateInvoiceAsync(UpdateInvoice invoice, CancellationToken cancellationToken);

    Task<ICollection<Invoice>> GetInvoicesAsync(CancellationToken cancellationToken);
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;

namespace AccountReceivableSystem.Domain.Repositories;

public interface IInvoiceRepository
{
    Task<Invoice> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken);

    Task<IList<Invoice>> GetUserInvoices(string userId, CancellationToken cancellationToken);

    Task UpdateInvoiceAsync(UpdateInvoice invoice, CancellationToken cancellationToken);

    Task<Invoice> GetAsync(string invoiceId, CancellationToken cancellationToken);
}
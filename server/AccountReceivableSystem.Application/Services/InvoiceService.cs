using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Application.Services.Abstractions;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Domain.UseCases.Abstractions;

namespace AccountReceivableSystem.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ICreateInvoiceUseCase _createInvoiceUseCase;
    private readonly IUpdateInvoiceUseCase _updateInvoiceUseCase;
    private readonly IGetInvoicesUseCase _getInvoicesUseCase;

    public InvoiceService(ICreateInvoiceUseCase createInvoiceUseCase,
        IUpdateInvoiceUseCase updateInvoiceUseCase,
        IGetInvoicesUseCase getInvoicesUseCase)
    {
        _createInvoiceUseCase = createInvoiceUseCase;
        _updateInvoiceUseCase = updateInvoiceUseCase;
        _getInvoicesUseCase = getInvoicesUseCase;
    }

    public Task<Invoice> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken)
        => _createInvoiceUseCase.CreateInvoiceAsync(invoice, cancellationToken);

    public Task UpdateInvoiceAsync(UpdateInvoice invoice, CancellationToken cancellationToken)
        => _updateInvoiceUseCase.UpdateInvoiceAsync(invoice, cancellationToken);

    public Task<ICollection<Invoice>> GetInvoicesAsync(CancellationToken cancellationToken)
        => _getInvoicesUseCase.GetAsync(cancellationToken);
}
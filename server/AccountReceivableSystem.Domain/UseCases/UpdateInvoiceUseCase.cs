using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Domain.Ports;
using AccountReceivableSystem.Domain.Repositories;
using AccountReceivableSystem.Domain.UseCases.Abstractions;

namespace AccountReceivableSystem.Domain.UseCases;

public class UpdateInvoiceUseCase : IUpdateInvoiceUseCase
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUserService _userService;

    public UpdateInvoiceUseCase(IInvoiceRepository invoiceRepository, IUserService userService)
    {
        _invoiceRepository = invoiceRepository;
        _userService = userService;
    }

    public async Task UpdateInvoiceAsync(UpdateInvoice invoice, CancellationToken cancellationToken)
    {
        var user = await _userService.GetCurrentUserAsync(cancellationToken);
        invoice.UserId = user.Id;

        await _invoiceRepository.UpdateInvoiceAsync(invoice, cancellationToken);
    }
}
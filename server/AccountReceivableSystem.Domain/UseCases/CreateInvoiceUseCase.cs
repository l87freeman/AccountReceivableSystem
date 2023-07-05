using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Domain.Ports;
using AccountReceivableSystem.Domain.Repositories;
using AccountReceivableSystem.Domain.UseCases.Abstractions;
using AccountReceivableSystem.Domain.Validators.Abstractions;

namespace AccountReceivableSystem.Domain.UseCases;

public class CreateInvoiceUseCase : ICreateInvoiceUseCase
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUserService _userService;
    private readonly IInvoiceValidator _invoiceValidator;

    public CreateInvoiceUseCase(IInvoiceRepository invoiceRepository, IUserService userService, IInvoiceValidator invoiceValidator)
    {
        _invoiceRepository = invoiceRepository;
        _userService = userService;
        _invoiceValidator = invoiceValidator;
    }

    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        _invoiceValidator.ValidateInvoice(invoice);

        var user = await _userService.GetCurrentUserAsync(cancellationToken);
        invoice.UserId = user.Id;

        var createdInvoice = await _invoiceRepository.CreateInvoiceAsync(invoice, cancellationToken);
        return createdInvoice;
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Domain.Ports;
using AccountReceivableSystem.Domain.Repositories;
using AccountReceivableSystem.Domain.UseCases.Abstractions;

namespace AccountReceivableSystem.Domain.UseCases;

public class GetInvoicesUseCase : IGetInvoicesUseCase
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUserService _userService;

    public GetInvoicesUseCase(IInvoiceRepository invoiceRepository, IUserService userService)
    {
        _invoiceRepository = invoiceRepository;
        _userService = userService;
    }

    public async Task<ICollection<Invoice>> GetAsync(CancellationToken cancellationToken)
    {
        var user = await _userService.GetCurrentUserAsync(cancellationToken);
        var invoices = await _invoiceRepository.GetUserInvoices(user.Id, cancellationToken);

        return invoices.OrderBy(x => x.DueDate).ToList();
    }
}
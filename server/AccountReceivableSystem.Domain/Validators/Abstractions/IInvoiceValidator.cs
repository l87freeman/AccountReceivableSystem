using AccountReceivableSystem.Domain.Entities;

namespace AccountReceivableSystem.Domain.Validators.Abstractions;

public interface IInvoiceValidator
{
    void ValidateInvoice(Invoice invoice);
}
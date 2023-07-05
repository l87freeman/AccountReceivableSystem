using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Domain.Exceptions;
using AccountReceivableSystem.Domain.Ports;
using AccountReceivableSystem.Domain.Validators.Abstractions;

namespace AccountReceivableSystem.Domain.Validators;

public class InvoiceValidator : IInvoiceValidator
{
    private readonly IDateTimeService _dateTimeService;

    public InvoiceValidator(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public void ValidateInvoice(Invoice invoice)
    {
        if (invoice == null)
        {
            throw new NotValidInvoiceException($"{nameof(Invoice)} is not valid");
        }

        ThrowOnEmptyString(invoice.InvoiceNumber, nameof(invoice.InvoiceNumber));
        ThrowOnEmptyString(invoice.CustomerName, nameof(invoice.CustomerName));

        ThrowOnNotValidDueDate(invoice);
        ThrowOnNotValidLineItems(invoice);
    }

    private static void ThrowOnNotValidLineItems(Invoice invoice)
    {
        if ((invoice.LineItems?.Count ?? 0) == 0)
        {
            throw new NotValidInvoiceException($"{nameof(invoice.LineItems)} must not be empty.");
        }

        foreach (var invoiceLineItem in invoice.LineItems)
        {
            ThrowOnNotValidLine(invoiceLineItem);
        }
    }

    private static void ThrowOnNotValidLine(LineItem invoiceLineItem)
    {
        ThrowOnEmptyString(invoiceLineItem.Description, nameof(LineItem.Description));

        if (invoiceLineItem.TotalPrice <= 0)
        {
            throw new NotValidInvoiceException($"{nameof(invoiceLineItem.TotalPrice)} is not valid - it has to be greater than 0.");
        }

        if (invoiceLineItem.Quantity <= 0)
        {
            throw new NotValidInvoiceException($"{nameof(invoiceLineItem.Quantity)} is not valid - it has to be greater than 0.");
        }
    }

    private void ThrowOnNotValidDueDate(Invoice invoice)
    {
        if (invoice.InvoiceDate >= invoice.DueDate || invoice.DueDate <= _dateTimeService.CurrentDateTime().AddDays(1).Date)
        {
            throw new NotValidInvoiceException($"{nameof(invoice.DueDate)} is not valid - it has to be greater than {nameof(invoice.InvoiceDate)} and must be greater than current date.");
        }
    }

    private static void ThrowOnEmptyString(string value, string name)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new NotValidInvoiceException($"{name} is not valid - it must not be an empty string.");
        }
    }
}
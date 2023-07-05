using System;

namespace AccountReceivableSystem.Domain.Exceptions;

public class CreateInvoiceConflictException : Exception
{
    public CreateInvoiceConflictException(string message) : base(message)
    {
        
    }
}
using System;

namespace AccountReceivableSystem.Domain.Exceptions;

public class NotValidInvoiceException : Exception
{
    public NotValidInvoiceException(string message) : base(message)
    {
        
    }
}
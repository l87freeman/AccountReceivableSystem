using System;

namespace AccountReceivableSystem.Domain.Exceptions;

public class InvoiceNotFoundException : Exception
{
    public InvoiceNotFoundException(string message) : base(message)
    {
        
    }
}
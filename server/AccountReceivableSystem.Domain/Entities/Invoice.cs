using System;
using System.Collections.Generic;

namespace AccountReceivableSystem.Domain.Entities;

public class Invoice
{
    public string Id { get; set; }

    public string CustomerName { get; set; }

    public string InvoiceNumber { get; set; }

    public DateTime InvoiceDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? PayDate { get; set; }

    public bool IsPaid { get; set; }

    public ICollection<LineItem> LineItems { get; set; }

    public string UserId { get; set; }
}
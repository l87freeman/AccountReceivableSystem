using System;
using System.Collections.Generic;

namespace AccountReceivableSystem.Web.Models.Response;

public record InvoiceResponse
{
    public string Id { get; init; }

    public string CustomerName { get; init; }

    public string InvoiceNumber { get; init; }

    public DateTime InvoiceDate { get; init; }

    public DateTime DueDate { get; init; }

    public DateTime? PayDate { get; init; }

    public bool IsPaid { get; set; }

    public ICollection<LineItem> LineItems { get; init; }
}
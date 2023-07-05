using System;
using System.Collections.Generic;

namespace AccountReceivableSystem.Web.Models.Request;

public class CreateInvoiceRequest
{
    public string CustomerName { get; set; }

    public string InvoiceNumber { get; set; }

    public DateTime InvoiceDate { get; set; }

    public DateTime DueDate { get; set; }

    public ICollection<LineItem> LineItems { get; init; }
}
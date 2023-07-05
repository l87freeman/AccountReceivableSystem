namespace AccountReceivableSystem.Web.Models.Request;

public record UpdateInvoiceRequest
{
    public string Id { get; init; }

    public bool IsPaid { get; init; }
}
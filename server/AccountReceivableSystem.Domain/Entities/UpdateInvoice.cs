namespace AccountReceivableSystem.Domain.Entities;

public class UpdateInvoice
{
    public string Id { get; set; }

    public bool IsPaid { get; set; }

    public string UserId { get; set; }
}
namespace AccountReceivableSystem.Domain.Entities;

public class LineItem
{
    public string Description { get; init; }

    public int Quantity { get; init; }

    public decimal TotalPrice { get; init; }
}
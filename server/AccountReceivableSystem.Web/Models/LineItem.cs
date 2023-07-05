namespace AccountReceivableSystem.Web.Models;

public record LineItem
{
    public string Description { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}
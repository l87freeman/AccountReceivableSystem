using AccountReceivableSystem.Domain.Entities;
using AutoFixture;

namespace AccountReceivableSystem.Domain.Tests.Customizations;

public class InvoiceLineItemRequestCustomizations : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LineItem>(composer =>
            composer
                .With(x => x.Quantity, () => 100)
                .With(x => x.TotalPrice, () => 500));
    }
}
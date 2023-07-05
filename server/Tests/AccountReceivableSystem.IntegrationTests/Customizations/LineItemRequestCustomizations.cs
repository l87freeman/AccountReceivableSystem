using AutoFixture;

namespace AccountReceivableSystem.IntegrationTests.Customizations;

public class LineItemRequestCustomizations : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Web.Models.LineItem>(composer =>
            composer
                .With(x => x.Quantity, () => 100)
                .With(x => x.TotalPrice, () => 500));
    }
}
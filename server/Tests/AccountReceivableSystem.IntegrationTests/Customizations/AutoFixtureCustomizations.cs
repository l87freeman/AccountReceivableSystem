using AutoFixture;

namespace AccountReceivableSystem.IntegrationTests.Customizations;

public class AutoFixtureCustomizations : CompositeCustomization
{
    public AutoFixtureCustomizations() : base(
        new CreateInvoiceRequestCustomizations(),
        new LineItemRequestCustomizations(),
        new InvoiceCustomizations(),
        new InvoiceLineItemRequestCustomizations())
    { }
}
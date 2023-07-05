using AutoFixture;

namespace AccountReceivableSystem.Domain.Tests.Customizations;

public class AutoFixtureCustomizations : CompositeCustomization
{
    public AutoFixtureCustomizations() : base(
        new InvoiceCustomizations(),
        new InvoiceLineItemRequestCustomizations())
    { }
}
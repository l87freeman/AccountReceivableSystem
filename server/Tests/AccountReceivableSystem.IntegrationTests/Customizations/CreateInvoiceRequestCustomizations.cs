using System;
using AutoFixture;

namespace AccountReceivableSystem.IntegrationTests.Customizations;

public class CreateInvoiceRequestCustomizations : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Web.Models.Request.CreateInvoiceRequest>(composer =>
            composer
                .With(x => x.InvoiceDate, () => DateTime.UtcNow.AddDays(-5))
                .With(x => x.DueDate, () => DateTime.UtcNow.AddDays(100)));
    }
}
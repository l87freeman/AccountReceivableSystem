using AccountReceivableSystem.Domain.Entities;
using AutoFixture;

namespace AccountReceivableSystem.Domain.Tests.Customizations;

public class InvoiceCustomizations : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Invoice>(composer =>
            composer
                .With(x => x.Id, () => fixture.Create<string>())
                .With(x => x.DueDate, () => DateTime.UtcNow.AddDays(2))
                .With(x => x.InvoiceDate, () => DateTime.UtcNow.AddDays(-2))
                .With(x => x.DueDate, () => DateTime.UtcNow));
    }
}
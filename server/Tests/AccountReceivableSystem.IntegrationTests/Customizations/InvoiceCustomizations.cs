using System;
using AccountReceivableSystem.Domain.Entities;
using AutoFixture;
using MongoDB.Bson;

namespace AccountReceivableSystem.IntegrationTests.Customizations;

public class InvoiceCustomizations : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Invoice>(composer =>
            composer
                .With(x => x.Id, () => ObjectId.GenerateNewId().ToString())
                .With(x => x.DueDate, () => DateTime.UtcNow.AddDays(2))
                .With(x => x.InvoiceDate, () => DateTime.UtcNow.AddDays(-2))
                .With(x => x.DueDate, () => DateTime.UtcNow));
    }
}
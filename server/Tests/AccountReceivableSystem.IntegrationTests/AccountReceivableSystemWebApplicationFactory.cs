using System.Collections.Generic;
using AccountReceivableSystem.Domain.Ports;
using AccountReceivableSystem.Infrastructure.Adapters;
using AccountReceivableSystem.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mongo2Go;
using Moq;

namespace AccountReceivableSystem.IntegrationTests;

public class AccountReceivableSystemWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly MongoDbRunner _mongoRunner;
    public const string SecretKey = "theKeyForTestsAndForVerificationThisKeyWontBeUsedAnywhereElse";

    public Mock<IDateTimeService> TimeService { get; } = new();

    public AccountReceivableSystemWebApplicationFactory()
    {
        _mongoRunner = MongoDbRunner.StartForDebugging();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(c => c.AddInMemoryCollection(new List<KeyValuePair<string, string>>
        {
            new("MongoDb:ConnectionString", _mongoRunner.ConnectionString),
            new("MongoDb:DatabaseName", "testInvoicesDb"),
            new("MongoDb:InvoiceCollectionName", "invoices"),
            new("Jwt:Key", SecretKey),
        }));

        builder.ConfigureServices((b, s) =>
        {
            s.Remove(new ServiceDescriptor(typeof(IDateTimeService), typeof(DateTimeService), ServiceLifetime.Scoped));
            s.AddSingleton<IDateTimeService>(TimeService.Object);
        });

        return base.CreateHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _mongoRunner.Dispose();
    }
}
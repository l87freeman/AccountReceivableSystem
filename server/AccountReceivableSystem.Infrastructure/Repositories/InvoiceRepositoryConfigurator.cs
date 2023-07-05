using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Infrastructure.Repositories.Abstractions;
using MongoDB.Driver;

namespace AccountReceivableSystem.Infrastructure.Repositories;

public class InvoiceRepositoryConfigurator : IDatabaseConfigurator
{
    private readonly IMongoCollection<Invoice> _invoiceCollection;

    public InvoiceRepositoryConfigurator(IMongoCollection<Invoice> invoiceCollection)
    {
        _invoiceCollection = invoiceCollection;
    }

    async Task IDatabaseConfigurator.ConfigureAsync()
    {
        var userId = Builders<Invoice>.IndexKeys.Ascending(x => x.UserId);
        var dueDate = Builders<Invoice>.IndexKeys.Ascending(x => x.DueDate);

        var userIdDueDate = Builders<Invoice>.IndexKeys.Combine(userId, dueDate);

        var customerName = Builders<Invoice>.IndexKeys.Ascending(x => x.CustomerName);
        var invoiceNumber = Builders<Invoice>.IndexKeys.Ascending(x => x.InvoiceNumber);
        var invoiceDate = Builders<Invoice>.IndexKeys.Ascending(x => x.InvoiceDate);

        var customerNameInvoice = Builders<Invoice>.IndexKeys.Combine(userId, customerName, invoiceNumber, invoiceDate);

        await _invoiceCollection.Indexes.CreateOneAsync(new CreateIndexModel<Invoice>(userIdDueDate, new CreateIndexOptions{ Unique = false, Background = true } ));
        await _invoiceCollection.Indexes.CreateOneAsync(new CreateIndexModel<Invoice>(customerNameInvoice, new CreateIndexOptions { Unique = true }));
    }
}
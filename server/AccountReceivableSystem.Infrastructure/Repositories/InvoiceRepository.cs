using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Domain.Exceptions;
using AccountReceivableSystem.Domain.Repositories;
using MongoDB.Driver;

namespace AccountReceivableSystem.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly IMongoCollection<Invoice> _invoiceCollection;

    public InvoiceRepository(IMongoCollection<Invoice> invoiceCollection)
    {
        _invoiceCollection = invoiceCollection;
    }

    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        try
        {
            await _invoiceCollection.InsertOneAsync(invoice, new InsertOneOptions(), cancellationToken);
            return invoice;
        }
        catch (MongoWriteException e) when (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            throw new CreateInvoiceConflictException("Invoice is not unique, please check invoice attributes");
        }
    }

    public async Task<IList<Invoice>> GetUserInvoices(string userId, CancellationToken cancellationToken)
    {
        var result = await _invoiceCollection
            .Find(invoice => invoice.UserId == userId)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<Invoice> GetAsync(string invoiceId, CancellationToken cancellationToken)
    {
        var result = await _invoiceCollection
            .Find(invoice => invoice.Id == invoiceId)
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task UpdateInvoiceAsync(UpdateInvoice invoice, CancellationToken cancellationToken)
    {
        var filter = Builders<Invoice>.Filter.And(
            Builders<Invoice>.Filter.Eq(x => x.UserId, invoice.UserId),
            Builders<Invoice>.Filter.Eq(x => x.Id, invoice.Id));

        var udpate = Builders<Invoice>.Update.Set(x => x.IsPaid, invoice.IsPaid)
            .Set(x => x.PayDate, DateTime.UtcNow);

        var result = await _invoiceCollection.UpdateOneAsync(filter, udpate, cancellationToken: cancellationToken);
        if (result.ModifiedCount == 0)
        {
            throw new InvoiceNotFoundException("Invoice with provided attributes was not found");
        }
    }
}
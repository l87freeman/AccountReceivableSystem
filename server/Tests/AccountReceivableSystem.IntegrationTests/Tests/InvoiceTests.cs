using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.IntegrationTests.Extensions;
using AccountReceivableSystem.IntegrationTests.Fixures;
using AccountReceivableSystem.Web.Models.Request;
using AccountReceivableSystem.Web.Models.Response;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AccountReceivableSystem.IntegrationTests.Tests;

public class InvoiceTests : BaseTestFixture
{
    private const string BaseUrl = "/api/invoices";

    public InvoiceTests(AccountReceivableSystemWebApplicationFactory factory) : base(factory)
    { }

    [Fact]
    public async Task CreateInvoice_WhenModelIsValid_ThenCreateNewInvoiceEntry()
    {
        var request = Autofixture.Create<CreateInvoiceRequest>();
        
        var storedInvoice = await HttpClient.PostAsJsonReadResponseAsync<CreateInvoiceRequest, InvoiceResponse>(BaseUrl, request);

        using (new AssertionScope())
        {
            (storedInvoice?.Id).Should().NotBeNullOrEmpty();
            storedInvoice.Should().BeEquivalentTo(request);

            var dbInvoice = await InvoiceRepository.GetAsync(storedInvoice!.Id, CancellationToken.None);
            dbInvoice.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task CreateInvoice_WhenModelIsDuplicate_ThenBadRequestResponse()
    {
        var invoice = await SetupUserInvoice();

        var request = Autofixture.Create<CreateInvoiceRequest>();
        request.DueDate = invoice.DueDate;
        request.InvoiceDate = invoice.InvoiceDate;
        request.InvoiceNumber = invoice.InvoiceNumber;
        request.CustomerName = invoice.CustomerName;

        var errorResponse = await HttpClient.PostAsJsonReadErrorResponseAsync(BaseUrl, request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (errorResponse.Error?.Message).Should().Be("Invoice is not unique, please check invoice attributes");
        }
    }

    [Fact]
    public async Task CreateInvoice_WhenNotAuthorized_ThenNotAuthorizedResponse()
    {
        var request = Autofixture.Create<CreateInvoiceRequest>();

        var errorResponse = await HttpNotAuthorizedClient.PostAsJsonReadErrorResponseAsync(BaseUrl, request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    [Theory]
    [MemberData(nameof(NotValidDueDateFeed))]
    public async Task CreateInvoice_WhenDueDateIsNotValid_ThenBadRequestResponse(int numb, DateTime invoiceDate, DateTime dueDate, DateTime currentDate)
    {
        var request = Autofixture.Create<CreateInvoiceRequest>();
        request.InvoiceDate = invoiceDate;
        request.DueDate = dueDate;

        TimeService.Setup(ts => ts.CurrentDateTime()).Returns(currentDate);

        var errorResponse = await HttpClient.PostAsJsonReadErrorResponseAsync(BaseUrl, request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (errorResponse.Error?.Message).Should().Be("DueDate is not valid - it has to be greater than InvoiceDate and must be greater than current date.");
        }
    }

    [Fact]
    public async Task CreateInvoice_WhenInvoiceNumberIsNotValid_ThenBadRequestResponse()
    {
        var request = Autofixture.Create<CreateInvoiceRequest>();
        request.InvoiceNumber = null;

        var errorResponse = await HttpClient.PostAsJsonReadErrorResponseAsync(BaseUrl, request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (errorResponse.Error?.Message).Should().Be("InvoiceNumber is not valid - it must not be an empty string.");
        }
    }

    [Fact]
    public async Task CreateInvoice_WhenCustomerNameIsNotValid_ThenBadRequestResponse()
    {
        var request = Autofixture.Create<CreateInvoiceRequest>();
        request.CustomerName = null;

        var errorResponse = await HttpClient.PostAsJsonReadErrorResponseAsync(BaseUrl, request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (errorResponse.Error?.Message).Should().Be("CustomerName is not valid - it must not be an empty string.");
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task CreateInvoice_WhenQuantityIsNotValid_ThenBadRequestResponse(int quantity)
    {
        var request = Autofixture.Create<CreateInvoiceRequest>();
        request.LineItems.ElementAt(0).Quantity = quantity;
            
        var errorResponse = await HttpClient.PostAsJsonReadErrorResponseAsync(BaseUrl, request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (errorResponse.Error?.Message).Should().Be("Quantity is not valid - it has to be greater than 0.");
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task CreateInvoice_WhenTotalPriceIsNotValid_ThenBadRequestResponse(int quantity)
    {
        var request = Autofixture.Create<CreateInvoiceRequest>();
        request.LineItems.ElementAt(0).TotalPrice = quantity;

        var errorResponse = await HttpClient.PostAsJsonReadErrorResponseAsync(BaseUrl, request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (errorResponse.Error?.Message).Should().Be("TotalPrice is not valid - it has to be greater than 0.");
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateInvoice_WhenDescriptionIsNotValid_ThenBadRequestResponse(string descr)
    {
        var request = Autofixture.Create<CreateInvoiceRequest>();
        request.LineItems.ElementAt(0).Description = descr;

        var errorResponse = await HttpClient.PostAsJsonReadErrorResponseAsync(BaseUrl, request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (errorResponse.Error?.Message).Should().Be("Description is not valid - it must not be an empty string.");
        }
    }

    [Fact]
    public async Task UpdateInvoice_WhenInvoiceExists_ThenUpdateInvoice()
    {
        var invoice = await SetupUserInvoice();
        
        var request = new UpdateInvoiceRequest { IsPaid = true, Id = invoice.Id };

        await HttpClient.PutAsJsonReadResponseAsync<UpdateInvoiceRequest, object>($"{BaseUrl}/{invoice.Id}", request);

        using (new AssertionScope())
        {
            var dbInvoice = await InvoiceRepository.GetAsync(invoice.Id, CancellationToken.None);
            dbInvoice.IsPaid.Should().BeTrue();
            dbInvoice.PayDate.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task UpdateInvoice_WhenInvoiceForOtherUser_ThenNotFoundResponse()
    {
        var invoice = await SetupUserInvoice();
        SetupAuthHttpClient(Autofixture.Create<string>());

        var request = new UpdateInvoiceRequest { IsPaid = true, Id = invoice.Id };

        var errorResponse = await HttpClient.PutAsJsonReadErrorResponseAsync($"{BaseUrl}/{invoice.Id}", request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            (errorResponse.Error?.Message).Should().Be("Invoice with provided attributes was not found");
        }
    }

    [Fact]
    public async Task UpdateInvoice_WhenNotAuthorized_ThenNotAuthorizedResponse()
    {
        var invoice = await SetupUserInvoice();

        var request = new UpdateInvoiceRequest { IsPaid = true, Id = invoice.Id };

        var errorResponse = await HttpNotAuthorizedClient.PutAsJsonReadErrorResponseAsync($"{BaseUrl}/{invoice.Id}", request);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    [Fact]
    public async Task GetInvoices_WhenInvoicesExist_ThenReturnInvoicesInAscendingOrder()
    {
        var invoicesCnt = 10;
        var userId = Autofixture.Create<string>();

        var invoices = await CreateInvoices(invoicesCnt, userId);
        SetupAuthHttpClient(userId);

        var response = await HttpClient.GetReadResponseAsync<ICollection<InvoiceResponse>>(BaseUrl);

        using (new AssertionScope())
        {
            response.Count.Should().Be(invoicesCnt);
            response.Should().BeInAscendingOrder(x => x.DueDate);
            foreach (var invoice in response)
            {
                invoices.Any(i => i.Id == invoice.Id
                                  && i.IsPaid == invoice.IsPaid
                                  && i.InvoiceNumber == invoice.InvoiceNumber
                                  && i.DueDate.Date == invoice.DueDate.Date
                                  && i.InvoiceDate.Date == invoice.InvoiceDate.Date).Should()
                    .BeTrue("Created and retrieved invoices have to match");
            }
        }
    }

    [Fact]
    public async Task GetInvoices_WhenInvoicesDoNotExistForUser_ThenReturnEmptyCollection()
    {
        var invoicesCnt = 10;
        var userId = Autofixture.Create<string>();

        await CreateInvoices(invoicesCnt, userId);
        SetupAuthHttpClient(Autofixture.Create<string>());

        var response = await HttpClient.GetReadResponseAsync<ICollection<InvoiceResponse>>(BaseUrl);

        using (new AssertionScope())
        {
            response.Count.Should().Be(0);
        }
    }

    [Fact]
    public async Task GetInvoices_WhenNotAuthorized_ThenNotAuthorizedResponse()
    {
        var errorResponse = await HttpNotAuthorizedClient.GetReadErrorResponseAsync(BaseUrl);

        using (new AssertionScope())
        {
            errorResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    private async Task<List<Invoice>> CreateInvoices(int cnt = 5, string userId = null, bool isPaid = false)
    {
        var result = new List<Invoice>();
        while (cnt-- > 0)
        {
            var invoice = await CreateInvoiceAsync(userId, isPaid);
            result.Add(invoice);
        }

        return result;
    }

    private async Task<Invoice> SetupUserInvoice()
    {
        var userId = Autofixture.Create<string>();
        var invoice = await CreateInvoiceAsync(userId);
        SetupAuthHttpClient(userId);

        return invoice;
    }

    private async Task<Invoice> CreateInvoiceAsync(string userId = null, bool isPaid = false)
    {
        var invoice = Autofixture.Create<Invoice>();
        invoice.UserId = userId ?? Autofixture.Create<string>();
        invoice.IsPaid = isPaid;

        await InvoiceRepository.CreateInvoiceAsync(invoice, CancellationToken.None);

        return invoice;
    }

    public static IEnumerable<object[]> NotValidDueDateFeed()
    {
        yield return new object[] { 1, DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(1), DateTime.UtcNow };
        yield return new object[] { 2, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1), DateTime.UtcNow };
    }
}
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Domain.Exceptions;
using AccountReceivableSystem.Domain.Ports;
using AccountReceivableSystem.Domain.Tests.Customizations;
using AccountReceivableSystem.Domain.Validators;
using AutoFixture;
using FluentAssertions;
using Moq;
using Xunit;

namespace AccountReceivableSystem.Domain.Tests.Validators;

public class InvoiceValidatorTests
{
    private Mock<IDateTimeService> _dateTimeService;
    private readonly InvoiceValidator _invoiceValidator;
    private readonly Fixture _fixture;

    public InvoiceValidatorTests()
    {
        _dateTimeService = new Mock<IDateTimeService>();
        _dateTimeService.Setup(x => x.CurrentDateTime()).Returns(DateTime.UtcNow.AddDays(-1));
        _invoiceValidator = new InvoiceValidator(_dateTimeService.Object);
        _fixture = new Fixture();
        _fixture.Customize(new AutoFixtureCustomizations());
    }

    [Fact]
    public void ValidateInvoice_WhenInvoiceNull_ThenValidationException()
    {
        var exception = Assert.Throws<NotValidInvoiceException>(() =>
         {
             _invoiceValidator.ValidateInvoice(null);
         });

        exception.Message.Should().Be("Invoice is not valid");
    }

    [Theory]
    [MemberData(nameof(InvalidInvoiceDataFeed))]
    public void ValidateInvoice_WhenInvoiceIsNotValid_ThenValidationException(int num, Action<Invoice> setup, string message)
    {
        var invoice = _fixture.Create<Invoice>();
        setup(invoice);
        var exception = Assert.Throws<NotValidInvoiceException>(() =>
        {
            _invoiceValidator.ValidateInvoice(invoice);
        });

        exception.Message.Should().Be(message);
    }

    public static IEnumerable<object[]> InvalidInvoiceDataFeed()
    {
        yield return new object[] {1, Create(i =>
        {
            i.DueDate = DateTime.UtcNow.AddDays(-1);
            i.InvoiceDate = DateTime.UtcNow;
        }), "DueDate is not valid - it has to be greater than InvoiceDate and must be greater than current date." };

        yield return new object[] {2, Create(i =>
        {
            i.DueDate = DateTime.UtcNow.AddDays(-4);
            i.InvoiceDate = DateTime.UtcNow.AddDays(-5);
        }), "DueDate is not valid - it has to be greater than InvoiceDate and must be greater than current date." };

        yield return new object[] {3, Create(i =>
        {
            i.DueDate = DateTime.UtcNow.AddDays(-1);
            i.InvoiceDate = DateTime.UtcNow.AddDays(-2);
        }), "DueDate is not valid - it has to be greater than InvoiceDate and must be greater than current date." };

        yield return new object[] {4, Create(i => i.InvoiceNumber = null), "InvoiceNumber is not valid - it must not be an empty string." };
        yield return new object[] {5, Create(i => i.InvoiceNumber = ""), "InvoiceNumber is not valid - it must not be an empty string." };
        yield return new object[] {6, Create(i => i.CustomerName = ""), "CustomerName is not valid - it must not be an empty string." };
        yield return new object[] {7, Create(i => i.LineItems = null), "LineItems must not be empty." };
        yield return new object[] {8, Create(i => i.LineItems = new List<LineItem>()), "LineItems must not be empty." };

        Action<Invoice> Create(Action<Invoice> action) => action;
    }
}
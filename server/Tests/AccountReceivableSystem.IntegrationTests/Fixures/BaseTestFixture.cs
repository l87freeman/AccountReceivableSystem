using System.IdentityModel.Tokens.Jwt;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using AccountReceivableSystem.Domain.Ports;
using AccountReceivableSystem.Domain.Repositories;
using AccountReceivableSystem.IntegrationTests.Customizations;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace AccountReceivableSystem.IntegrationTests.Fixures;

public class BaseTestFixture : IClassFixture<AccountReceivableSystemWebApplicationFactory>
{
    public BaseTestFixture(AccountReceivableSystemWebApplicationFactory factory)
    {
        Factory = factory;
        Autofixture = new();
        Autofixture.Customize(new AutoFixtureCustomizations());

        InvoiceRepository = Factory.Services.GetRequiredService<IInvoiceRepository>();
        SetupAuthHttpClient();

        HttpNotAuthorizedClient = Factory.CreateClient();
    }

    protected Mock<IDateTimeService> TimeService => Factory.TimeService;

    protected AccountReceivableSystemWebApplicationFactory Factory { get; }

    protected Fixture Autofixture { get; } 

    protected IInvoiceRepository InvoiceRepository { get; }

    protected HttpClient HttpClient { get; private set; }

    protected HttpClient HttpNotAuthorizedClient { get; }

    protected void SetupAuthHttpClient(string userId = null)
    {
        HttpClient = Factory.CreateClient();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenerateToken(userId));
    }

    private string GenerateToken(string userId = null)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccountReceivableSystemWebApplicationFactory.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Autofixture.Create<string>()),
            new Claim(ClaimTypes.Name,Autofixture.Create<string>()),
            new Claim(ClaimTypes.Sid,userId ?? Autofixture.Create<string>()),
            new Claim(ClaimTypes.Role,"user")
        };
        var token = new JwtSecurityToken("",
            "",
            claims,
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
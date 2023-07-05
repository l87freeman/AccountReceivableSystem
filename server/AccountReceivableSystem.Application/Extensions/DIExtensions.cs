using AccountReceivableSystem.Application.Services;
using AccountReceivableSystem.Application.Services.Abstractions;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Domain.Ports;
using AccountReceivableSystem.Domain.Repositories;
using AccountReceivableSystem.Domain.UseCases;
using AccountReceivableSystem.Domain.UseCases.Abstractions;
using AccountReceivableSystem.Domain.Validators;
using AccountReceivableSystem.Domain.Validators.Abstractions;
using AccountReceivableSystem.Infrastructure.Adapters;
using AccountReceivableSystem.Infrastructure.HostedServices;
using AccountReceivableSystem.Infrastructure.Repositories;
using AccountReceivableSystem.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace AccountReceivableSystem.Application.Extensions;

public static class DIExtensions
{
    public static IServiceCollection AddMongoDbCollections(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MongoDb:ConnectionString"];
        var databaseName = configuration["MongoDb:DatabaseName"];
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

        services.AddSingleton(s => s.GetRequiredService<IMongoClient>().GetDatabase(databaseName));


        services.AddSingleton(s =>
            s.GetRequiredService<IMongoDatabase>()
                .GetCollection<Invoice>(configuration["MongoDb:InvoiceCollectionName"]));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IInvoiceRepository, InvoiceRepository>();
        services.AddSingleton<IDatabaseConfigurator, InvoiceRepositoryConfigurator>();
        return services;
    }

    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateInvoiceUseCase, CreateInvoiceUseCase>();
        services.AddScoped<IGetInvoicesUseCase, GetInvoicesUseCase>();
        services.AddScoped<IUpdateInvoiceUseCase, UpdateInvoiceUseCase>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<IInvoiceValidator, InvoiceValidator>();

        services.AddHostedService<DatabaseConfigurationHostedService>();

        return services;
    }
}
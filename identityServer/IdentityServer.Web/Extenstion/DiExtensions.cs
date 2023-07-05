using IdentityServer.Application.Entities;
using IdentityServer.Application.Repositories.Abstractions;
using IdentityServer.Application.Repositories;
using IdentityServer.Application.Services.Abstractions;
using IdentityServer.Application.Services;
using IdentityServer.Web.HostedServices;
using MongoDB.Driver;

namespace IdentityServer.Web.Extenstion;

internal static class DiExtensions
{
    public static IServiceCollection AddMongoDbCollections(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MongoDb:ConnectionString"];
        var databaseName = configuration["MongoDb:DatabaseName"];
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

        services.AddSingleton(s => s.GetRequiredService<IMongoClient>().GetDatabase(databaseName));


        services.AddSingleton(s =>
            s.GetRequiredService<IMongoDatabase>()
                .GetCollection<UserIdentity>(configuration["MongoDb:UserIdentityCollectionName"]));

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IHashService, HashService>();

        services.AddSingleton<IDatabaseConfigurator, UserDatabaseConfigurator>();
        services.AddHostedService<DatabaseConfigurationHostedService>();

        return services;
    }
}
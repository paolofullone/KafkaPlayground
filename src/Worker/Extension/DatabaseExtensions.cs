using Infrastructure.DbFactory;
using Infrastructure.DbFactory.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Worker.Extension;
public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISQLConnectionFactory>(_ =>
            new SQLConnectionFactory(configuration.GetConnectionString("SqlServer")?.Trim()!));

        services.AddSingleton<IOracleConnectionFactory>(_ =>
            new OracleConnectionFactory(configuration.GetConnectionString("Oracle")?.Trim()!));

        services.AddSingleton<IDbSQLRepository, SQLRepository>();
        services.AddSingleton<IDbOracleRepository, OracleRepository>();

        return services;
    }
}


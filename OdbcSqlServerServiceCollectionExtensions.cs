using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ODBC.SQLServer;
public static class OdbcSqlServerServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkOdbcSqlServer(this IServiceCollection services)
    {
        SqlServerServiceCollectionExtensions.AddEntityFrameworkSqlServer(services);
        services.AddSingleton<IRelationalConnection, OdbcRelationalConnection>();

        return services;
    }
}


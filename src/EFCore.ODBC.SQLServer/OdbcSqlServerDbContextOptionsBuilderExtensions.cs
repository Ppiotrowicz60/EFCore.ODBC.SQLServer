using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace EFCore.ODBC.SQLServer;
public static class OdbcSqlServerDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseOdbcSqlServer(
        this DbContextOptionsBuilder optionsBuilder,
        string odbcConnectionString,
        Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
    {
#pragma warning disable EF1001 // Internal EF Core API usage.
        optionsBuilder.UseSqlServer(odbcConnectionString, sqlServerOptionsAction)
            .ReplaceService<ISqlServerConnection, OdbcSqlServerRelationalConnection>()
            .ReplaceService<IRelationalCommandBuilderFactory, OdbcSqlServerRelationalCommandBuilderFactory>()
            .ReplaceService<IRelationalTypeMappingSource, OdbcSqlServerTypeMappingSource>();
#pragma warning restore EF1001 // Internal EF Core API usage.

        return optionsBuilder;
    }
}


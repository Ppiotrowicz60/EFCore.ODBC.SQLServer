using EFCore.ODBC.SqlServer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ODBC.SQLServer;
public static class OdbcSqlServerDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseOdbcSqlServer(
        this DbContextOptionsBuilder builder, string odbcConnectionString)
    {
        var extension = builder.Options.FindExtension<OdbcSqlServerOptionsExtension>()
            ?? new OdbcSqlServerOptionsExtension();

        extension.ConnectionString = odbcConnectionString;

        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(extension);
        return builder;
    }
}


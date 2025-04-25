using EFCore.ODBC.SqlServer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Options;

namespace EFCore.ODBC.SQLServer;
public static class OdbcSqlServerDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseOdbcSqlServer(
        this DbContextOptionsBuilder optionsBuilder, string odbcConnectionString)
    {
        var extension = (OdbcSqlServerOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnectionString(odbcConnectionString);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        return optionsBuilder;
    }

    private static OdbcSqlServerOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.Options.FindExtension<OdbcSqlServerOptionsExtension>()
        ?? new OdbcSqlServerOptionsExtension();
}


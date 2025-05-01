using EFCore.ODBC.SqlServer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ODBC.SQLServer;
public static class OdbcSqlServerDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseOdbcSqlServer(
        this DbContextOptionsBuilder optionsBuilder,
        string odbcConnectionString,
        Action<OdbcSqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
    {
        var extension = (OdbcSqlServerOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnectionString(odbcConnectionString);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        return ApplyConfiguration(optionsBuilder, sqlServerOptionsAction);
    }

    private static OdbcSqlServerOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.Options.FindExtension<OdbcSqlServerOptionsExtension>()
        ?? new OdbcSqlServerOptionsExtension();

    private static DbContextOptionsBuilder ApplyConfiguration(
        DbContextOptionsBuilder optionsBuilder,
        Action<OdbcSqlServerDbContextOptionsBuilder>? sqlServerOptionsAction)
    {
        sqlServerOptionsAction?.Invoke(new OdbcSqlServerDbContextOptionsBuilder(optionsBuilder));

        var extension = (OdbcSqlServerOptionsExtension)GetOrCreateExtension(optionsBuilder).ApplyDefaults(optionsBuilder.Options);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}


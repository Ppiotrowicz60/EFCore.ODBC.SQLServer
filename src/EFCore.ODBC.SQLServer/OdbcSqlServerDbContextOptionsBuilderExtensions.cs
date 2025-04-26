using EFCore.ODBC.SqlServer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ODBC.SQLServer;
public static class OdbcSqlServerDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseOdbcSqlServer(
        this DbContextOptionsBuilder optionsBuilder,
        string odbcConnectionString,
        Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction = null)
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
        Action<SqlServerDbContextOptionsBuilder>? sqlServerOptionsAction)
    {
        sqlServerOptionsAction?.Invoke(new SqlServerDbContextOptionsBuilder(optionsBuilder));

        var extension = (OdbcSqlServerOptionsExtension)GetOrCreateExtension(optionsBuilder).ApplyDefaults(optionsBuilder.Options);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}


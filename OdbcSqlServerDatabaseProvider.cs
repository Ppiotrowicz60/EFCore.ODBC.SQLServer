using EFCore.ODBC.SqlServer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ODBC.SQLServer;
public class OdbcSqlServerDatabaseProvider : IDatabaseProvider
{
    public string Name => "EFCore.ODBC.SqlServer";

    public bool IsConfigured(IDbContextOptions options)
        => options.Extensions.OfType<OdbcSqlServerOptionsExtension>().Any();
}


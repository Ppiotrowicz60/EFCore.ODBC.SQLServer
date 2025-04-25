using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.Odbc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ODBC.SQLServer;
public class OdbcRelationalConnection : RelationalConnection, ISqlServerConnection
{
    public OdbcRelationalConnection(RelationalConnectionDependencies dependencies)
        : base(dependencies) { }

    protected override DbConnection CreateDbConnection()
    {
        return new OdbcConnection(ConnectionString!);
    }

    public virtual ISqlServerConnection CreateMasterConnection()
    {
        var connectionStringBuilder = new OdbcConnectionStringBuilder(ConnectionString);
        connectionStringBuilder.Remove("AttachDBFilename");

        var contextOptions = new DbContextOptionsBuilder()
            .UseOdbcSqlServer(
                connectionStringBuilder.ConnectionString)
            .Options;

        return new OdbcRelationalConnection(Dependencies with { ContextOptions = contextOptions });
    }

    public virtual bool IsMultipleActiveResultSetsEnabled
        => false;

    protected override bool SupportsAmbientTransactions
        => true;
}

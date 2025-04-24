using System.Data.Common;
using System.Data.Odbc;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ODBC.SQLServer;
public class OdbcRelationalConnection : RelationalConnection
{
    public OdbcRelationalConnection(RelationalConnectionDependencies dependencies)
        : base(dependencies) { }

    protected override DbConnection CreateDbConnection()
    {
        return new OdbcConnection(ConnectionString);
    }
}


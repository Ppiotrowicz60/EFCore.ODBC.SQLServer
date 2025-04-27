using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ODBC.SQLServer;

public class OdbcSqlServerRelationalCommandBuilderFactory : RelationalCommandBuilderFactory
{
    public OdbcSqlServerRelationalCommandBuilderFactory(RelationalCommandBuilderDependencies dependencies)
        : base(dependencies)
    {
    }

    public override IRelationalCommandBuilder Create()
    {
        return new OdbcSqlServerRelationalCommandBuilder(Dependencies);
    }
}

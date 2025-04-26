using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ODBC.SQLServer;

public class OdbcRelationalCommandBuilderFactory : RelationalCommandBuilderFactory
{
    public OdbcRelationalCommandBuilderFactory(RelationalCommandBuilderDependencies dependencies)
        : base(dependencies)
    {
    }

    public override IRelationalCommandBuilder Create()
    {
        return new OdbcRelationalCommandBuilder(Dependencies);
    }
}

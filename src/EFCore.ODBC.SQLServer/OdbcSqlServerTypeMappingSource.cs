using System.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
#pragma warning disable EF1001 // Internal EF Core API usage.

public class OdbcSqlServerTypeMappingSource : SqlServerTypeMappingSource
{
    public OdbcSqlServerTypeMappingSource(
        TypeMappingSourceDependencies dependencies,
        RelationalTypeMappingSourceDependencies relationalDependencies)
        : base(dependencies, relationalDependencies)
    {
    }

    private static readonly SqlServerDateTimeTypeMapping _smallDatetime
        = new("smalldatetime", DbType.DateTime);
    protected override RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        if (mappingInfo.StoreTypeName?.ToLowerInvariant() == "smalldatetime")
        {
            return _smallDatetime;
        }

        return base.FindMapping(mappingInfo);
    }
}
#pragma warning restore EF1001 // Internal EF Core API usage.

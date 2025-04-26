using Microsoft.EntityFrameworkCore.Update;

namespace EFCore.ODBC.SQLServer.Tests;

public class TestModificationCommandBatch : SingularModificationCommandBatch
{
    public TestModificationCommandBatch(
        ModificationCommandBatchFactoryDependencies dependencies,
        int? maxBatchSize)
        : base(dependencies)
    {
        MaxBatchSize = maxBatchSize ?? 42;
    }

    protected override int MaxBatchSize { get; }
}
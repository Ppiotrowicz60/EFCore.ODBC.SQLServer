using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update;

namespace EFCore.ODBC.SQLServer.Tests;

public class TestModificationCommandBatchFactory : IModificationCommandBatchFactory
{
    private readonly ModificationCommandBatchFactoryDependencies _dependencies;
    private readonly IDbContextOptions _options;

    public TestModificationCommandBatchFactory(
        ModificationCommandBatchFactoryDependencies dependencies,
        IDbContextOptions options)
    {
        _dependencies = dependencies;
        _options = options;
    }

    public int CreateCount { get; private set; }

    public virtual ModificationCommandBatch Create()
    {
        CreateCount++;

        var optionsExtension = _options.Extensions.OfType<FakeRelationalOptionsExtension>().FirstOrDefault();

        return new TestModificationCommandBatch(_dependencies, optionsExtension?.MaxBatchSize);
    }
}

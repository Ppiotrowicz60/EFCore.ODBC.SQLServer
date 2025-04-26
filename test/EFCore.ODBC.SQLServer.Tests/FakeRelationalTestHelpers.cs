using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ODBC.SQLServer.Tests;

public class FakeRelationalTestHelpers : TestHelpers
{
    protected FakeRelationalTestHelpers()
    {
    }

    public static FakeRelationalTestHelpers Instance { get; } = new();

    protected virtual EntityFrameworkDesignServicesBuilder CreateEntityFrameworkDesignServicesBuilder(
        IServiceCollection services)
        => new EntityFrameworkRelationalDesignServicesBuilder(services);

    public override IServiceCollection AddProviderServices(IServiceCollection services)
        => FakeRelationalOptionsExtension.AddEntityFrameworkRelationalDatabase(services);

    public override DbContextOptionsBuilder UseProviderOptions(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseOdbcSqlServer("Driver={ODBC Driver 17 for SQL Server};Server=localhost;Database=TestDb;Trusted_Connection=Yes;");

    public override LoggingDefinitions LoggingDefinitions { get; } = new TestRelationalLoggingDefinitions();
}



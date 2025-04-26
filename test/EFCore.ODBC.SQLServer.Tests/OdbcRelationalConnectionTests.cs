using System.Data.Odbc;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.Extensions.Logging;
using Xunit;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.TestUtilities;

namespace EFCore.ODBC.SQLServer.Tests;
#pragma warning disable EF1001 // Internal EF Core API usage.
public class OdbcRelationalConnectionTests
{
    [Fact]
    public void CreateDbConnection_ShouldReturnOdbcConnection()
    {
        var connectionString = "Driver={ODBC Driver 17 for SQL Server};Server=localhost;Database=TestDb;Trusted_Connection=Yes";

        var options = new DbContextOptionsBuilder()
            .UseOdbcSqlServer(connectionString)
            .Options;

        using var connection = new OdbcRelationalConnection(CreateDependencies(options));

        Assert.NotNull(connection);
        Assert.IsType<OdbcRelationalConnection>(connection);
        Assert.Equal(connectionString, connection.ConnectionString);
    }

    [Fact]
    public void CreateMasterConnection_ShouldReturnOdbcConnection()
    {
        var connectionString = "Driver={ODBC Driver 17 for SQL Server};Server=localhost;Database=TestDb;Trusted_Connection=Yes";

        var options = new DbContextOptionsBuilder()
            .UseOdbcSqlServer(connectionString)
            .Options;

        using var connection = new OdbcRelationalConnection(CreateDependencies(options));
        using var masterConnection = connection.CreateMasterConnection();

        Assert.NotNull(masterConnection);
        Assert.IsType<OdbcRelationalConnection>(masterConnection);
        Assert.IsType<ISqlServerConnection>(masterConnection, false);
        Assert.Equal(connectionString?.ToLower(), masterConnection.ConnectionString?.ToLower());
        Assert.Equal(60, masterConnection.CommandTimeout);
    }

    [Fact]
    public void CreateMasterConnectionWithTimeout_ShouldReturnOdbcConnectionWithTimeout()
    {
        var connectionString = "Driver={ODBC Driver 17 for SQL Server};Server=localhost;Database=TestDb;Trusted_Connection=Yes";

        var options = new DbContextOptionsBuilder()
            .UseOdbcSqlServer(connectionString,
                b => b.CommandTimeout(55))
            .Options;

        using var connection = new OdbcRelationalConnection(CreateDependencies(options));
        using var masterConnection = connection.CreateMasterConnection();

        Assert.NotNull(masterConnection);
        Assert.IsType<OdbcRelationalConnection>(masterConnection);
        Assert.Equal(connectionString?.ToLower(), masterConnection.ConnectionString?.ToLower());
        Assert.Equal(55, masterConnection.CommandTimeout);
    }

    public static RelationalConnectionDependencies CreateDependencies(DbContextOptions options = null)
    {
        options ??= new DbContextOptionsBuilder()
            .UseOdbcSqlServer(@"Driver={ODBC Driver 17 for SQL Server};Server=localhost;Database=TestDb;Trusted_Connection=Yes;")
            .Options;

        return new RelationalConnectionDependencies(
            options,
            new DiagnosticsLogger<DbLoggerCategory.Database.Transaction>(
                new LoggerFactory(),
                new LoggingOptions(),
                new DiagnosticListener("FakeDiagnosticListener"),
                new SqlServerLoggingDefinitions(),
                new NullDbContextLogger()),
            new RelationalConnectionDiagnosticsLogger(
                new LoggerFactory(),
                new LoggingOptions(),
                new DiagnosticListener("FakeDiagnosticListener"),
                new SqlServerLoggingDefinitions(),
                new NullDbContextLogger(),
                CreateOptions()),
            new NamedConnectionStringResolver(options),
            new RelationalTransactionFactory(
                new RelationalTransactionFactoryDependencies(
                    new RelationalSqlGenerationHelper(
                        new RelationalSqlGenerationHelperDependencies()))),
            new CurrentDbContext(new FakeDbContext()),
            new RelationalCommandBuilderFactory(
                new RelationalCommandBuilderDependencies(
                    new TestRelationalTypeMappingSource(
                        TestServiceFactory.Instance.Create<TypeMappingSourceDependencies>(),
                        TestServiceFactory.Instance.Create<RelationalTypeMappingSourceDependencies>()),
                    new SqlServerExceptionDetector())));
    }

    private const string ConnectionString = "Fake Connection String";

    private static IDbContextOptions CreateOptions(
        RelationalOptionsExtension? optionsExtension = null)
    {
        var optionsBuilder = new DbContextOptionsBuilder();

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder)
            .AddOrUpdateExtension(
                optionsExtension
                ?? new FakeRelationalOptionsExtension().WithConnectionString(ConnectionString));

        return optionsBuilder.Options;
    }

    private class FakeDbContext : DbContext
    {
    }
}
#pragma warning restore EF1001 // Internal EF Core API usage.

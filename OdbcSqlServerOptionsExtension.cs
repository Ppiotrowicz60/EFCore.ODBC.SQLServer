using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using EFCore.ODBC.SQLServer;

namespace EFCore.ODBC.SqlServer;

public class OdbcSqlServerOptionsExtension : IDbContextOptionsExtension
{
    public string ConnectionString { get; set; }

    public DbContextOptionsExtensionInfo Info => new ExtensionInfo(this);

    public void ApplyServices(IServiceCollection services)
    {
        services.AddEntityFrameworkOdbcSqlServer();
    }

    public void Validate(IDbContextOptions options) { }

    private class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension) { }

        public override string LogFragment => "Using ODBC SQL Server Provider";

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) { }

        public override bool IsDatabaseProvider => false; // Implementing the missing property  
    }
}

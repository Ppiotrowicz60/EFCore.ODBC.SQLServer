using EFCore.ODBC.SQLServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class OdbcRelationalConnectionTestsFixture : IDisposable
{
    public const string ConnectionString = "dsn=testOdbcSqlServer;uid=sa;pwd=YourStrong!Passw0rd;";

    public ServiceProvider ServiceProvider { get; }

    public OdbcRelationalConnectionTestsFixture()
    {
        var services = new ServiceCollection();

        services
            .AddDbContext<TestDbContext>(options => options.UseOdbcSqlServer(ConnectionString))
            .AddEntityFrameworkSqlServer()
            .AddEntityFrameworkOdbcSqlServer();

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider?.Dispose();
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}

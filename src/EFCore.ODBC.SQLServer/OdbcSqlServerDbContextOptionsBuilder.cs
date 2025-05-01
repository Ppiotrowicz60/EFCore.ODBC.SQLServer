using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ODBC.SqlServer;

public class OdbcSqlServerDbContextOptionsBuilder
    : RelationalDbContextOptionsBuilder<OdbcSqlServerDbContextOptionsBuilder, OdbcSqlServerOptionsExtension>
{
    public OdbcSqlServerDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        : base(optionsBuilder)
    {
    }

    public virtual OdbcSqlServerDbContextOptionsBuilder EnableRetryOnFailure()
        => ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c));

    public virtual OdbcSqlServerDbContextOptionsBuilder EnableRetryOnFailure(int maxRetryCount)
        => ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c, maxRetryCount));

    public virtual OdbcSqlServerDbContextOptionsBuilder EnableRetryOnFailure(ICollection<int> errorNumbersToAdd)
        => ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c, errorNumbersToAdd));

    public virtual OdbcSqlServerDbContextOptionsBuilder EnableRetryOnFailure(
        int maxRetryCount,
        TimeSpan maxRetryDelay,
        IEnumerable<int>? errorNumbersToAdd)
        => ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c, maxRetryCount, maxRetryDelay, errorNumbersToAdd));

    public virtual OdbcSqlServerDbContextOptionsBuilder UseCompatibilityLevel(int compatibilityLevel)
        => WithOption(e => e.WithCompatibilityLevel(compatibilityLevel));

    public virtual OdbcSqlServerDbContextOptionsBuilder UseAzureSqlDefaults(bool enable = true)
        => WithOption(e => e.WithAzureSql(enable));
}
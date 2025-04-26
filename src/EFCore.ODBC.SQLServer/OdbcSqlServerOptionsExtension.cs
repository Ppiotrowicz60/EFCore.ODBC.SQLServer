using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text;
using EFCore.ODBC.SQLServer;

namespace EFCore.ODBC.SqlServer;

public class OdbcSqlServerOptionsExtension : RelationalOptionsExtension, IDbContextOptionsExtension
{
    private DbContextOptionsExtensionInfo? _info;
    private int? _compatibilityLevel;
    private bool? _azureSql;

    public static readonly int DefaultCompatibilityLevel = 160;

    public OdbcSqlServerOptionsExtension()
    {
    }

    protected OdbcSqlServerOptionsExtension(OdbcSqlServerOptionsExtension copyFrom)
        : base(copyFrom)
    {
        _compatibilityLevel = copyFrom._compatibilityLevel;
        _azureSql = copyFrom._azureSql;
    }

    public override DbContextOptionsExtensionInfo Info
        => _info ??= new ExtensionInfo(this);
    protected override RelationalOptionsExtension Clone()
        => new OdbcSqlServerOptionsExtension(this);
    public virtual int CompatibilityLevel
        => _compatibilityLevel ?? DefaultCompatibilityLevel;
    public virtual int? CompatibilityLevelWithoutDefault
        => _compatibilityLevel;

    public virtual OdbcSqlServerOptionsExtension WithCompatibilityLevel(int? compatibilityLevel)
    {
        var clone = (OdbcSqlServerOptionsExtension)Clone();

        clone._compatibilityLevel = compatibilityLevel;

        return clone;
    }

    public virtual bool IsAzureSql
        => _azureSql ?? false;
    public virtual OdbcSqlServerOptionsExtension WithAzureSql(bool enable)
    {
        var clone = (OdbcSqlServerOptionsExtension)Clone();

        clone._azureSql = enable;

        return clone;
    }

    public virtual IDbContextOptionsExtension ApplyDefaults(IDbContextOptions options)
    {
        if (!IsAzureSql)
        {
            return this;
        }

        if (ExecutionStrategyFactory == null)
        {
            return WithExecutionStrategyFactory(c => new SqlServerRetryingExecutionStrategy(c));
        }

        return this;
    }

    public override void ApplyServices(IServiceCollection services)
        => services.AddEntityFrameworkOdbcSqlServer();

    private sealed class ExtensionInfo : RelationalExtensionInfo
    {
        private string? _logFragment;

        public ExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        private new OdbcSqlServerOptionsExtension Extension
            => (OdbcSqlServerOptionsExtension)base.Extension;

        public override bool IsDatabaseProvider
            => true;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            => other is ExtensionInfo otherInfo
                && Extension.CompatibilityLevel == otherInfo.Extension.CompatibilityLevel;

        public override string LogFragment
        {
            get
            {
                if (_logFragment == null)
                {
                    var builder = new StringBuilder();

                    builder.Append(base.LogFragment);

                    if (Extension._compatibilityLevel is int compatibilityLevel)
                    {
                        builder
                            .Append("CompatibilityLevel=")
                            .Append(compatibilityLevel);
                    }

                    _logFragment = builder.ToString();
                }

                return _logFragment;
            }
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["SqlServer"] = "1";

            if (Extension.CompatibilityLevel is int compatibilityLevel)
            {
                debugInfo["CompatibilityLevel"] = compatibilityLevel.ToString();
            }
        }
    }
}

using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ODBC.SQLServer;

public class OdbcSqlServerRelationalCommandBuilder : RelationalCommandBuilder
{
    private static readonly Regex PlaceholderRegex = new(@"(?<!@)@([A-Za-z0-9_]+)", RegexOptions.Compiled);

    public OdbcSqlServerRelationalCommandBuilder(RelationalCommandBuilderDependencies dependencies)
        : base(dependencies)
    {
        Dependencies = dependencies;
    }

    protected override RelationalCommandBuilderDependencies Dependencies { get; }

    public override IRelationalCommand Build()
    {
        var command = (RelationalCommand)base.Build();

        var sql = command.CommandText;
        var parameters = command.Parameters;

        var paramByName = new Dictionary<string, IRelationalParameter>(StringComparer.Ordinal);
        foreach (var parameter in parameters)
        {
            var key = parameter.InvariantName.StartsWith("@", StringComparison.Ordinal)
                ? parameter.InvariantName.Substring(1)
                : parameter.InvariantName;
            paramByName[key] = parameter;
        }

        var orderedParams = new List<IRelationalParameter>();

        var rebuilt = PlaceholderRegex.Replace(sql, match =>
        {
            var name = match.Groups[1].Value;
            if (TryResolveParameter(paramByName, name, out var parameter))
            {
                orderedParams.Add(parameter);
                return "?";
            }
            return match.Value;
        });

        return new RelationalCommand(
            Dependencies,
            rebuilt,
            Guid.NewGuid().ToString(),
            orderedParams
        );
    }

    private static bool TryResolveParameter(
        Dictionary<string, IRelationalParameter> paramByName,
        string name,
        out IRelationalParameter parameter)
    {
        if (paramByName.TryGetValue(name, out parameter!))
        {
            return true;
        }

        var normalized = name;
        while (normalized.Length > 0 && (normalized[0] == '_' || char.IsDigit(normalized[0])))
        {
            normalized = normalized.Substring(1);
        }

        if (normalized.Length > 0 && paramByName.TryGetValue(normalized, out parameter!))
        {
            return true;
        }

        foreach (var kvp in paramByName)
        {
            if (name.StartsWith(kvp.Key, StringComparison.Ordinal) || name.EndsWith(kvp.Key, StringComparison.Ordinal))
            {
                parameter = kvp.Value;
                return true;
            }
        }

        parameter = null!;
        return false;
    }
}

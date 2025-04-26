using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ODBC.SQLServer;

public class OdbcRelationalCommandBuilder : RelationalCommandBuilder
{
    public OdbcRelationalCommandBuilder(RelationalCommandBuilderDependencies dependencies)
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

        var orderedParams = new List<IRelationalParameter>();

        foreach (var parameter in parameters)
        {
            var parameterName = parameter.InvariantName.StartsWith("@", StringComparison.Ordinal)
                ? parameter.InvariantName
                : "@" + parameter.InvariantName;

            if (sql.Contains(parameterName))
            {
                sql = sql.Replace(parameterName, "?");
                orderedParams.Add(parameter);
            }
        }

        return new RelationalCommand(
            Dependencies,
            sql,
            orderedParams
        );
    }

    private static string ReplaceFirst(string text, string search, string replace)
    {
        var pos = text.IndexOf(search, StringComparison.Ordinal);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }
}

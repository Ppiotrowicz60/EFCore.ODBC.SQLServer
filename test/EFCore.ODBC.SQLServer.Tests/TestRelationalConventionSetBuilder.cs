using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EFCore.ODBC.SQLServer.Tests;

public class TestRelationalConventionSetBuilder : RelationalConventionSetBuilder
{
    public TestRelationalConventionSetBuilder(
        ProviderConventionSetBuilderDependencies dependencies,
        RelationalConventionSetBuilderDependencies relationalDependencies)
        : base(dependencies, relationalDependencies)
    {
    }

    public static ConventionSet Build()
        => ConventionSet.CreateConventionSet(FakeRelationalTestHelpers.Instance.CreateContext());
}



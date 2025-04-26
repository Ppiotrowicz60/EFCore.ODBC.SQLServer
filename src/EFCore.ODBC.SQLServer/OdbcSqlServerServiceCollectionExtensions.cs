using EFCore.ODBC.SqlServer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ODBC.SQLServer;
public static class OdbcSqlServerServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkOdbcSqlServer(this IServiceCollection services)
    {
#pragma warning disable EF1001 // Internal EF Core API usage.
        new EntityFrameworkRelationalServicesBuilder(services)
                    .TryAdd<LoggingDefinitions, SqlServerLoggingDefinitions>()
                    .TryAdd<IDatabaseProvider, DatabaseProvider<OdbcSqlServerOptionsExtension>>()
                    .TryAdd<IValueGeneratorCache>(p => p.GetRequiredService<ISqlServerValueGeneratorCache>())
                    .TryAdd<IRelationalTypeMappingSource, SqlServerTypeMappingSource>()
                    .TryAdd<ISqlGenerationHelper, SqlServerSqlGenerationHelper>()
                    .TryAdd<IRelationalAnnotationProvider, SqlServerAnnotationProvider>()
                    .TryAdd<IMigrationsAnnotationProvider, SqlServerMigrationsAnnotationProvider>()
                    .TryAdd<IModelValidator, SqlServerModelValidator>()
                    .TryAdd<IProviderConventionSetBuilder, SqlServerConventionSetBuilder>()
                    .TryAdd<IUpdateSqlGenerator>(p => p.GetRequiredService<ISqlServerUpdateSqlGenerator>())
                    .TryAdd<IEvaluatableExpressionFilter, SqlServerEvaluatableExpressionFilter>()
                    .TryAdd<IRelationalTransactionFactory, SqlServerTransactionFactory>()
                    .TryAdd<IModificationCommandBatchFactory, SqlServerModificationCommandBatchFactory>()
                    .TryAdd<IModificationCommandFactory, SqlServerModificationCommandFactory>()
                    .TryAdd<IValueGeneratorSelector, SqlServerValueGeneratorSelector>()
                    .TryAdd<IRelationalConnection>(p => p.GetRequiredService<ISqlServerConnection>())
                    .TryAdd<IMigrationsSqlGenerator, SqlServerMigrationsSqlGenerator>()
                    .TryAdd<IRelationalDatabaseCreator, SqlServerDatabaseCreator>()
                    .TryAdd<IHistoryRepository, SqlServerHistoryRepository>()
                    .TryAdd<IExecutionStrategyFactory, SqlServerExecutionStrategyFactory>()
                    .TryAdd<IRelationalQueryStringFactory, SqlServerQueryStringFactory>()
                    .TryAdd<ICompiledQueryCacheKeyGenerator, SqlServerCompiledQueryCacheKeyGenerator>()
                    .TryAdd<IQueryCompilationContextFactory, SqlServerQueryCompilationContextFactory>()
                    .TryAdd<IMethodCallTranslatorProvider, SqlServerMethodCallTranslatorProvider>()
                    .TryAdd<IAggregateMethodCallTranslatorProvider, SqlServerAggregateMethodCallTranslatorProvider>()
                    .TryAdd<IMemberTranslatorProvider, SqlServerMemberTranslatorProvider>()
                    .TryAdd<IQuerySqlGeneratorFactory, SqlServerQuerySqlGeneratorFactory>()
                    .TryAdd<IRelationalSqlTranslatingExpressionVisitorFactory, SqlServerSqlTranslatingExpressionVisitorFactory>()
                    .TryAdd<ISqlExpressionFactory, SqlServerSqlExpressionFactory>()
                    .TryAdd<IQueryTranslationPostprocessorFactory, SqlServerQueryTranslationPostprocessorFactory>()
                    .TryAdd<IRelationalParameterBasedSqlProcessorFactory, SqlServerParameterBasedSqlProcessorFactory>()
                    .TryAdd<INavigationExpansionExtensibilityHelper, SqlServerNavigationExpansionExtensibilityHelper>()
                    .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, SqlServerQueryableMethodTranslatingExpressionVisitorFactory>()
                    .TryAdd<IExceptionDetector, SqlServerExceptionDetector>()
                    .TryAdd<ISingletonOptions, ISqlServerSingletonOptions>(p => p.GetRequiredService<ISqlServerSingletonOptions>())
                    .TryAddProviderSpecificServices(
                        b => b
                            .TryAddSingleton<ISqlServerSingletonOptions, SqlServerSingletonOptions>()
                            .TryAddSingleton<ISqlServerValueGeneratorCache, SqlServerValueGeneratorCache>()
                            .TryAddSingleton<ISqlServerUpdateSqlGenerator, SqlServerUpdateSqlGenerator>()
                            .TryAddSingleton<ISqlServerSequenceValueGeneratorFactory, SqlServerSequenceValueGeneratorFactory>()
                            .TryAddScoped<ISqlServerConnection, OdbcRelationalConnection>())
                    .TryAddCoreServices();
#pragma warning restore EF1001 // Internal EF Core API usage.

        return services;
    }
}

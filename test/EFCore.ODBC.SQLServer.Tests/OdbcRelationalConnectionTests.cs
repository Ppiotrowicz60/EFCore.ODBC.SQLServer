using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ODBC.SQLServer.Tests;

public class OdbcRelationalConnectionIntegrationTests : IClassFixture<OdbcRelationalConnectionTestsFixture>
{
    private readonly OdbcRelationalConnectionTestsFixture _fixture;

    public OdbcRelationalConnectionIntegrationTests(OdbcRelationalConnectionTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void CanOpenAndCloseConnection()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var connection = dbContext.Database.GetDbConnection();

        Assert.NotNull(connection);
        Assert.Equal(OdbcRelationalConnectionTestsFixture.ConnectionString, connection.ConnectionString);

        connection.Open();
        Assert.Equal(ConnectionState.Open, connection.State);

        connection.Close();
        Assert.Equal(ConnectionState.Closed, connection.State);
    }

    [Fact]
    public void CanExecuteQuery()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var connection = dbContext.Database.GetDbConnection();

        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1";

        var result = command.ExecuteScalar();

        Assert.NotNull(result);
        Assert.Equal(1, Convert.ToInt32(result));

        connection.Close();
        Assert.Equal(ConnectionState.Closed, connection.State);
    }

    [Fact]
    public void CanExecuteSimpleSelectQuery()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var connection = dbContext.Database.GetDbConnection();

        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT 42";

        var result = command.ExecuteScalar();

        Assert.NotNull(result);
        Assert.Equal(42, Convert.ToInt32(result));

        connection.Close();
        Assert.Equal(ConnectionState.Closed, connection.State);
    }

    [Fact]
    public void CanExecuteNotNamedParameterizedQuery()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var connection = dbContext.Database.GetDbConnection();

        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT ?";

        var parameter = command.CreateParameter();
        parameter.Value = 100;
        command.Parameters.Add(parameter);

        var result = command.ExecuteScalar();

        Assert.NotNull(result);
        Assert.Equal(100, Convert.ToInt32(result));

        connection.Close();
        Assert.Equal(ConnectionState.Closed, connection.State);
    }

    [Fact]
    public void CanExecuteNamedParameterizedQuery()
    {
        // Won't work, tests using linq queries are more important!
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var connection = dbContext.Database.GetDbConnection();

        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT @value1, @value2, @value3";
        var parameter1 = command.CreateParameter();
        parameter1.ParameterName = "@value1";
        parameter1.Value = 100;
        command.Parameters.Add(parameter1);

        var parameter2 = command.CreateParameter();
        parameter2.ParameterName = "@value2";
        parameter2.Value = 200;
        command.Parameters.Add(parameter2);

        var parameter3 = command.CreateParameter();
        parameter3.ParameterName = "@value3";
        parameter3.Value = 300;
        command.Parameters.Add(parameter3);

        var result = command.ExecuteScalar();

        Assert.NotNull(result);
        Assert.Equal(100, Convert.ToInt32(result));

        connection.Close();
        Assert.Equal(ConnectionState.Closed, connection.State);
    }

    [Fact]
    public void CanExecuteNamedParameterizedLinqQuery()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var id = 1;

        // Perform a simple LINQ query  
        var result = dbContext.Users
            .Where(u => u.Id == id)
            .FirstOrDefault();

        Assert.NotNull(result);
        Assert.Equal("tester1", result.Name);
    }

    [Fact]
    public void CanExecuteNamedParameterizedLinqQueryComplex()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var id = 1;
        var name = "tester1";

        // Perform a simple LINQ query  
        var result = dbContext.Users
            .Where(u => u.Id == id && u.Name == name)
            .FirstOrDefault();

        Assert.NotNull(result);
        Assert.Equal("tester1", result.Name);
    }

    [Fact]
    public void CanExecuteNamedParameterizedAndLimitedLinqQueryComplex()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var name = "tester";
        var count = 3;

        // Perform a simple LINQ query  
        var result = dbContext.Users
            .Where(u => u.Name.StartsWith(name))
            .Take(count);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("tester1", result.FirstOrDefault()?.Name);
    }

    [Fact]
    public void CanExecuteLinqQuery()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();

        // Perform a simple LINQ query  
        var result = dbContext.Users
            .Where(u => u.Id == 1)
            .FirstOrDefault();

        Assert.NotNull(result);
        Assert.Equal("tester1", result.Name);
    }

    [Fact]
    public void CanExecuteNamedParameterizedLinqQuerySQL()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var id = 1;

        // Perform a simple LINQ query  
        var result = (from u in dbContext.Users
                      where u.Id == id
                      select u).FirstOrDefault();

        Assert.NotNull(result);
        Assert.Equal("tester1", result.Name);
    }

    [Fact]
    public void CanExecuteNamedParameterizedLinqQueryComplexSQL()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var id = 1;
        var name = "tester1";

        // Perform a simple LINQ query  
        var result = (from u in dbContext.Users
                      where u.Id == id && u.Name == name
                      select u).FirstOrDefault();

        Assert.NotNull(result);
        Assert.Equal("tester1", result.Name);
    }

    [Fact]
    public void CanExecuteNamedParameterizedAndLimitedLinqQueryComplexSQL()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var name = "tester";
        var count = 3;

        // Perform a simple LINQ query  
        var result = (from u in dbContext.Users
                      where u.Name.StartsWith(name)
                      select u).Take(count);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("tester1", result.FirstOrDefault()?.Name);
    }

    [Fact]
    public void CanExecuteLinqQuerySQL()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();

        // Perform a simple LINQ query  
        var result = (from u in dbContext.Users
                      where u.Id == 1
                      select u).FirstOrDefault();

        Assert.NotNull(result);
        Assert.Equal("tester1", result.Name);
    }

    [Fact]
    public void CanExecuteQueryReturningMultipleRows()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var connection = dbContext.Database.GetDbConnection();

        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1 UNION SELECT 2 UNION SELECT 3";

        using var reader = command.ExecuteReader();
        var results = new List<int>();

        while (reader.Read())
        {
            results.Add(reader.GetInt32(0));
        }

        Assert.Equal(new[] { 1, 2, 3 }, results);

        connection.Close();
        Assert.Equal(ConnectionState.Closed, connection.State);
    }

    [Fact]
    public void CanExecuteQueryWithJoin()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var connection = dbContext.Database.GetDbConnection();

        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"
           WITH Data1 AS (SELECT 1 AS Id, 'A' AS Value),
                Data2 AS (SELECT 1 AS Id, 'B' AS Value)
           SELECT d1.Value, d2.Value
           FROM Data1 d1
           INNER JOIN Data2 d2 ON d1.Id = d2.Id";

        using var reader = command.ExecuteReader();
        Assert.True(reader.Read());
        Assert.Equal("A", reader.GetString(0));
        Assert.Equal("B", reader.GetString(1));

        connection.Close();
        Assert.Equal(ConnectionState.Closed, connection.State);
    }

    [Fact]
    public void CanExecuteQueryWithAggregateFunction()
    {
        using var scope = _fixture.ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OdbcRelationalConnectionTestsFixture.TestDbContext>();
        var connection = dbContext.Database.GetDbConnection();

        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT SUM(Value) FROM (VALUES (10), (20), (30)) AS Data(Value)";

        var result = command.ExecuteScalar();

        Assert.NotNull(result);
        Assert.Equal(60, Convert.ToInt32(result));

        connection.Close();
        Assert.Equal(ConnectionState.Closed, connection.State);
    }

}

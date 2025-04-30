using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace EFCore.ODBC.SQLServer.Tests;
public class LocalServiceTests
{

    protected IHost Host { get; set; }

    protected LocalTestService LocalTestService { get; set; }

    [TearDown]
    public void TearDown()
    {
        Host.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .Build();
    }

    [SetUp]
    public void SetupService()
    {
        var connectionString = "dsn=testOdbcSqlServer;uid=sa;pwd=YourStrong!Passw0rd;";

        var builder = new DbContextOptionsBuilder<LocalContext>();
        builder.UseOdbcSqlServer(connectionString);

        LocalTestService = new LocalTestService(new LocalContext(builder.Options));
    }


    [Test]
    public async Task GetComplexEntityAsync_ReturnsEntity_WhenIdExists()
    {
        var result = await LocalTestService.GetComplexEntityAsync(1);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetEntitiesWithUnionAsync_ReturnsEntities_WhenIdsExist()
    {
        var result = await LocalTestService.GetEntitiesWithUnionAsync(1, 2);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetEntitiesWithMultipleUnionsAsync_ReturnsEntities_WhenIdsExist()
    {
        var result = await LocalTestService.GetEntitiesWithMultipleUnionsAsync(1, 2, 3);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetEntitiesWithUnionAndFilterAsync_ReturnsFilteredEntities_WhenIdsAndFilterMatch()
    {
        var result = await LocalTestService.GetEntitiesWithUnionAndFilterAsync(1, 2, "testValue");
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
        Assert.That(result.All(e => e.StringValue == "testValue"));
    }

    [Test]
    public async Task GetEntitiesWithUnionAndSortingAsync_ReturnsSortedEntities_WhenIdsExist()
    {
        var result = await LocalTestService.GetEntitiesWithUnionAndSortingAsync(1, 2);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
        Assert.That(result, Is.Ordered.By(nameof(ComplexEntity.IntValue)));
    }

    [Test]
    public async Task GetEntitiesByStringValuesAsync_ReturnsEntities_WhenStringValuesMatch()
    {
        var stringValues = new List<string> { "value1", "value2" };
        var result = await LocalTestService.GetEntitiesByStringValuesAsync(stringValues);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
        Assert.That(result.All(e => stringValues.Contains(e.StringValue)));
    }

}
